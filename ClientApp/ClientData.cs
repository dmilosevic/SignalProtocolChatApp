using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libsignal;
using libsignal.ecc;
using libsignal.state;
using libsignal.state.impl;
using libsignal.util;

namespace ClientApp
{
    public static class ClientData
    {
        #region Properties
        public static string Username { get; set; }
        public static uint RegistrationId { get; set; }
        public static uint DeviceId { get; set; }

        public static InMemorySignalProtocolStore InMemorySignalProtocolStore { get; set; }
        public static SignedPreKeyRecord SignedPreKeyRecord { get; set; }
        public static IList<PreKeyRecord> OTPreKeyRecords { get; set; }

        public static PreKeyBundle PreKeyBundle { get; set; }

        public static SessionBuilder SessionBuilder { get; set; }
        public static SessionCipher SessionCipher { get; set; }
        #endregion

        public static void GenerateKeys()
        {
            RegistrationId = KeyHelper.generateRegistrationId(false);
            DeviceId = KeyHelper.getRandomSequence(100000);

            InMemorySignalProtocolStore = new InMemorySignalProtocolStore(KeyHelper.generateIdentityKeyPair(), RegistrationId);

            SignedPreKeyRecord = KeyHelper.generateSignedPreKey(InMemorySignalProtocolStore.GetIdentityKeyPair(), KeyHelper.generateSenderKeyId());
            InMemorySignalProtocolStore.StoreSignedPreKey(SignedPreKeyRecord.getId(), SignedPreKeyRecord);

            OTPreKeyRecords = KeyHelper.generatePreKeys(1, 3); //probacu sa id = 1
            OTPreKeyRecords.ToList().ForEach(x => 
                InMemorySignalProtocolStore.StorePreKey(x.getId(), x));

        }

        /// <summary>
        /// Malo odstupanje od protokola. Klijent sam izabere jedan OTPK i posalje serveru, umesto liste svih OTPKs.
        /// </summary>
        public static void CreateKeyBundle()
        {
            PreKeyRecord otpk = OTPreKeyRecords.Count > 0 ? OTPreKeyRecords[0] : null;

            PreKeyBundle = new PreKeyBundle(
                RegistrationId,
                DeviceId,
                otpk == null ? default(uint) : otpk.getId(),
                otpk.getKeyPair().getPublicKey(),
                SignedPreKeyRecord.getId(),
                SignedPreKeyRecord.getKeyPair().getPublicKey(),
                SignedPreKeyRecord.getSignature(),
                InMemorySignalProtocolStore.GetIdentityKeyPair().getPublicKey()
                );
            
        }
    }
}
