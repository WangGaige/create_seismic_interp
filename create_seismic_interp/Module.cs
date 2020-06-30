using System;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace create_seismic_interp
{
    /// <summary>
    /// This class will control the lifecycle of the Module.
    /// The order of the methods are the same as the calling order.
    /// </summary>
    public class Module : IModule
    {
        private Process m_workstepInstance;
        public Module()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region IModule Members

        /// <summary>
        /// This method runs once in the Module life; when it loaded into the petrel.
        /// This method called first.
        /// </summary>
        public void Initialize()
        {
            // TODO:  Add Module.Initialize implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the not UI related components.
        /// (eg: datasource, plugin)
        /// </summary>
        public void Integrate()
        {
            // Register create_seismic_interp.Workstep
            create_seismic_interp.Workstep workstepInstance = new create_seismic_interp.Workstep();
            PetrelSystem.WorkflowEditor.Add(workstepInstance);
            m_workstepInstance = new Slb.Ocean.Petrel.Workflow.WorkstepProcessWrapper(workstepInstance);
            PetrelSystem.ProcessDiagram.Add(m_workstepInstance, "Plug-ins");

            // TODO:  Add Module.Integrate implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the UI related components.
        /// (eg: settingspages, treeextensions)
        /// </summary>
        public void IntegratePresentation()
        {

            // TODO:  Add Module.IntegratePresentation implementation
        }

        /// <summary>
        /// This method runs once in the Module life.
        /// right before the module is unloaded. 
        /// It usually happens when the application is closing.
        /// </summary>
        public void Disintegrate()
        {
            PetrelSystem.ProcessDiagram.Remove(m_workstepInstance);
            // TODO:  Add Module.Disintegrate implementation
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // TODO:  Add Module.Dispose implementation
        }

        #endregion

    }


}