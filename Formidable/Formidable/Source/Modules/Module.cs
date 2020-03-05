using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formidable.Modules
{

    public abstract class Module
    {

        protected ModuleInformation moduleInformation;

        public Module(ModuleInformation moduleInformation)
        {
            if (moduleInformation == null)
                throw new ArgumentNullException(nameof(moduleInformation));

            this.moduleInformation = moduleInformation;
        }

        public abstract void Toggle();

        public virtual void OnGUI()
        {

        }

    }

}
