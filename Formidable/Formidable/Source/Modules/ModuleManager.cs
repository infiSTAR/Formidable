using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Formidable.Modules
{
    
    public static class ModuleManager
    {

        private static readonly KeyCode _activationKeyCode = KeyCode.Keypad0;

        public static bool IsActivated
        {
            get => _isActivated;
        }

        private static bool _isInitialized;

        private static bool _isActivated;

        private static Dictionary<KeyCode, Module> _modules;

        static ModuleManager()
        {
            _isInitialized = false;
            _isActivated = true;
            _modules = null;
        }

        public static void Initialize()
        {
            if (_isInitialized)
                return;

            _modules = new Dictionary<KeyCode, Module>();

            _isInitialized = true;
        }

        public static void Update()
        {
            if (!_isInitialized)
                return;

            if (Input.GetKeyDown(_activationKeyCode))
                _isActivated = !_isActivated;

            if (!_isActivated)
                return;

            foreach (KeyValuePair<KeyCode, Module> moduleEntry in _modules)
            {
                if (Input.GetKeyDown(moduleEntry.Key))
                {
                    _modules[moduleEntry.Key].Toggle();
                }
            }
        }

        public static void OnGUI()
        {
            if (!_isInitialized)
                return;

            if (!_isActivated)
                return;

            foreach (KeyValuePair<KeyCode, Module> moduleEntry in _modules)
            {
                moduleEntry.Value.OnGUI();
            }
        }

        public static bool AddModule(KeyCode keyCode, Module module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));

            if (!_isInitialized)
                return false;

            if (_modules.ContainsKey(keyCode))
                return false;

            _modules.Add(keyCode, module);

            return true;
        }

    }

}
