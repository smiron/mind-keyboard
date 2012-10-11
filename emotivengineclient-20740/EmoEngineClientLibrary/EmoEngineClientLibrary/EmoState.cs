using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmoEngineClientLibrary
{
    public class EmoState
    {
        private object emoState;

        public EmoState(object p)
        {
            // TODO: Complete member initialization
            this.emoState = p;
        }
        public EmoState Clone()
        {
            throw new NotImplementedException();
        }

        public float AffectivGetEngagementBoredomScore()
        {
            throw new NotImplementedException();
        }

        public float ExpressivGetEyebrowExtent()
        {
            throw new NotImplementedException();
        }

        public float ExpressivGetClenchExtent()
        {
            throw new NotImplementedException();
        }

        public void ExpressivGetEyelidState(out float leftEye, out float rightEye)
        {
            throw new NotImplementedException();
        }

        public EE_ExpressivAlgo_t ExpressivGetLowerFaceAction()
        {
            throw new NotImplementedException();
        }

        public float ExpressivGetLowerFaceActionPower()
        {
            throw new NotImplementedException();
        }

        public float ExpressivGetSmileExtent()
        {
            throw new NotImplementedException();
        }

        public EE_ExpressivAlgo_t ExpressivGetUpperFaceAction()
        {
            throw new NotImplementedException();
        }

        public float ExpressivGetUpperFaceActionPower()
        {
            throw new NotImplementedException();
        }

        public bool ExpressivIsBlink()
        {
            throw new NotImplementedException();
        }

        public bool ExpressivIsEyesOpen()
        {
            throw new NotImplementedException();
        }

        public bool ExpressivIsLeftWink()
        {
            throw new NotImplementedException();
        }

        public bool ExpressivIsLookingDown()
        {
            throw new NotImplementedException();
        }

        public bool ExpressivIsLookingLeft()
        {
            throw new NotImplementedException();
        }

        public bool ExpressivIsLookingRight()
        {
            throw new NotImplementedException();
        }

        public bool ExpressivIsLookingUp()
        {
            throw new NotImplementedException();
        }

        public bool ExpressivIsRightWink()
        {
            throw new NotImplementedException();
        }

        public int GetHeadsetOn()
        {
            throw new NotImplementedException();
        }

        public int GetNumContactQualityChannels()
        {
            throw new NotImplementedException();
        }

        public float GetTimeFromStart()
        {
            throw new NotImplementedException();
        }

        public bool CognitivIsActive()
        {
            throw new NotImplementedException();
        }

        public float CognitivGetCurrentActionPower()
        {
            throw new NotImplementedException();
        }

        public float AffectivGetExcitementLongTermScore()
        {
            throw new NotImplementedException();
        }

        public float AffectivGetExcitementShortTermScore()
        {
            throw new NotImplementedException();
        }

        public float AffectivGetFrustrationScore()
        {
            throw new NotImplementedException();
        }

        public float AffectivGetMeditationScore()
        {
            throw new NotImplementedException();
        }

        public EE_CognitivAction_t CognitivGetCurrentAction()
        {
            throw new NotImplementedException();
        }

        public EE_SignalStrength_t GetWirelessSignalStatus()
        {
            throw new NotImplementedException();
        }

        public void GetBatteryChargeLevel(out int batteryChargeLevel, out int maxChargeLevel)
        {
            throw new NotImplementedException();
        }

        public EE_EEG_ContactQuality_t[] GetContactQualityFromAllChannels()
        {
            throw new NotImplementedException();
        }
    }
}
