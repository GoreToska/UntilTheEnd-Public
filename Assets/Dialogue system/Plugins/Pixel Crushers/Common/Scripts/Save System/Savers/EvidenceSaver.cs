using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace PixelCrushers
{
    public class EvidenceSaver : Saver // Rename this class.
    {
        [Tooltip("If set, save specified evidence. Otherwise save this evidence.")]
        [SerializeField]
        private WorldEvidence _evidence = null;

        [Serializable]
        public class EvidenceData
        {
            public PromptManager _promptManager;
            public Cinemachine.CinemachineVirtualCamera _cvc;
            public Camera _mainCamera;
            public InspectionCamera _inspectionCamera;
            public Volume _blur;
            public GameObject _inspectionCanvas;
            public GameObject _mainCanvas;
            public TMPro.TMP_Text _evidenceName;
            public string _dialogueVariable;
        }

        protected EvidenceData e_data;

        public WorldEvidence Evidence
        {
            get { return (_evidence == null) ? this.transform.GetComponent<WorldEvidence>() : _evidence; }
            set { _evidence = value; }
        }

        public override void Awake()
        {
            base.Awake();
            e_data = new EvidenceData();
        }

        public override string RecordData()
        {
            EvidenceData data = new EvidenceData();
            Debug.Log(Evidence);
            //data._evidenceName = Evidence.EvidenceItem.Name;
            data._dialogueVariable = Evidence._dialogueVariableName;

            return SaveSystem.Serialize(data);
        }

        public override void ApplyData(string s)
        {
            if (string.IsNullOrEmpty(s)) return; // No data to apply.
            EvidenceData data = SaveSystem.Deserialize<EvidenceData>(s);
            if (data == null) return; // Serialized string isn't valid.
                                      //(do something with data here)
            SetFields(data);
        }

        private void SetFields(EvidenceData data)
        {
            //Evidence._evidenceName = data._evidenceName;
            Evidence._dialogueVariableName = data._dialogueVariable;
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
