using System;
using UnityEngine;

namespace PixelCrushers
{
    public class LocationTransitionSaver : Saver
    {
        [Tooltip("If set, save specified prompt. Otherwise save this prompt.")]
        [SerializeField]
        private LoadSceneOnTrigger _loader = null;

        [Serializable]
        public class PromptData
        {
            public PromptManager _promptManager;
        }

        protected PromptData p_data;

        public LoadSceneOnTrigger Loader
        {
            get { return (_loader == null) ? this.transform.GetComponent<LoadSceneOnTrigger>() : _loader; }
            set { _loader = value; }
        }

        public override void Awake()
        {
            base.Awake();
            p_data = new PromptData();
        }

        public override string RecordData()
        {
            PromptData data = new PromptData();
            //data._promptManager = Loader._promptManager;
            return SaveSystem.Serialize(data);
        }

        public override void ApplyData(string s)
        {
            if (string.IsNullOrEmpty(s)) return; // No data to apply.
            PromptData data = SaveSystem.Deserialize<PromptData>(s);
            if (data == null) return; // Serialized string isn't valid.
                                      //(do something with data here)
            SetFields(data);
        }

        private void SetFields(PromptData data)
        {
            //Loader._promptManager = data._promptManager;
        }

        public override void ApplyDataImmediate()
        {
            // If your Saver needs to pull data from the Save System immediately after
            // loading a scene, instead of waiting for ApplyData to be called at its
            // normal time, which may be some number of frames after the scene has started,
            // it can implement this method. For efficiency, the Save System will not look up 
            // the Saver's data; your method must look it up manually by calling 
            // SaveSystem.savedGameData.GetData(key).
        }

        public override void OnBeforeSceneChange()
        {
            // The Save System will call this method before scene changes. If your saver listens for 
            // OnDisable or OnDestroy messages (see DestructibleSaver for example), it can use this 
            // method to ignore the next OnDisable or OnDestroy message since they will be called
            // because the entire scene is being unloaded.
        }

        public override void OnRestartGame()
        {
            // The Save System will call this method when restarting the game from the beginning.
            // Your Saver can reset things to a fresh state if necessary.
        }

    }

}
