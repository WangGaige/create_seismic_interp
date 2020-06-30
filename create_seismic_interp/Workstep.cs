using System;
using System.Collections.Generic;
using Slb.Ocean.Core;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace create_seismic_interp
{
    /// <summary>
    /// This class contains all the methods and subclasses of the Workstep.
    /// Worksteps are displayed in the workflow editor.
    /// </summary>
    class Workstep : Workstep<Workstep.Arguments>, IExecutorSource, IAppearance, IDescriptionSource
    {
        #region Overridden Workstep methods

        /// <summary>
        /// Creates an empty Argument instance
        /// </summary>
        /// <returns>New Argument instance.</returns>

        protected override Workstep.Arguments CreateArgumentPackageCore(IDataSourceManager dataSourceManager)
        {
            return new Arguments(dataSourceManager);
        }
        /// <summary>
        /// Copies the Arguments instance.
        /// </summary>
        /// <param name="fromArgumentPackage">the source Arguments instance</param>
        /// <param name="toArgumentPackage">the target Arguments instance</param>
        protected override void CopyArgumentPackageCore(Arguments fromArgumentPackage, Arguments toArgumentPackage)
        {
            DescribedArgumentsHelper.Copy(fromArgumentPackage, toArgumentPackage);
        }

        /// <summary>
        /// Gets the unique identifier for this Workstep.
        /// </summary>
        protected override string UniqueIdCore
        {
            get
            {
                return "8f56fff8-30ad-4db0-83fd-dd6e1cd5bd6b";
            }
        }
        #endregion

        #region IExecutorSource Members and Executor class

        /// <summary>
        /// Creates the Executor instance for this workstep. This class will do the work of the Workstep.
        /// </summary>
        /// <param name="argumentPackage">the argumentpackage to pass to the Executor</param>
        /// <param name="workflowRuntimeContext">the context to pass to the Executor</param>
        /// <returns>The Executor instance.</returns>
        public Slb.Ocean.Petrel.Workflow.Executor GetExecutor(object argumentPackage, WorkflowRuntimeContext workflowRuntimeContext)
        {
            return new Executor(argumentPackage as Arguments, workflowRuntimeContext);
        }

        public class Executor : Slb.Ocean.Petrel.Workflow.Executor
        {
            Arguments arguments;
            WorkflowRuntimeContext context;

            public Executor(Arguments arguments, WorkflowRuntimeContext context)
            {
                this.arguments = arguments;
                this.context = context;
            }

            public override void ExecuteSimple()
            {
                // TODO: Implement the workstep logic here.
                SeismicCube cube = arguments.Cube;
                SeismicCollection sc = cube.SeismicCollection;
                SeismicProject proj = null;
                SeismicRoot sroot;
                sroot = SeismicRoot.Get(PetrelProject.PrimaryProject);
                if (!sroot.HasSeismicProject) return;
                proj = sroot.SeismicProject;
                using (ITransaction t = DataManager.NewTransaction()) {
                    t.Lock(proj);
                    InterpretationCollection aaa= proj.CreateInterpretationCollection("aaa");
                    HorizonInterpretation h= aaa.CreateHorizonInterpretation("surface", cube.Domain);
                    HorizonInterpretation3D h3d = h.CreateHorizonInterpretation3D(sc);
                    List<HorizonInterpretation3DSample> horizonsamples = new List<HorizonInterpretation3DSample>();
                    for (int i = 0; i < cube.NumSamplesIJK.I; i++)
                    {
                        for (int j = 0; j < cube.NumSamplesIJK.J; j++)
                        {
                            horizonsamples.Add(new HorizonInterpretation3DSample(i, j, -2000));
                        }
                    }
                    h3d.Samples = horizonsamples;
                    t.Commit();
                }
            }
        }

        #endregion

        /// <summary>
        /// ArgumentPackage class for Workstep.
        /// Each public property is an argument in the package.  The name, type and
        /// input/output role are taken from the property and modified by any
        /// attributes applied.
        /// </summary>
        public class Arguments : DescribedArgumentsByReflection
        {
            private Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube cube;
            public Arguments()
                : this(DataManager.DataSourceManager)
            {                
            }
            [Description("Cube", "Seismic cube to extract data from")]
            public Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube Cube
            {
                internal get { return this.cube; }
                set { this.cube = value; }
            }
            public Arguments(IDataSourceManager dataSourceManager)
            {
            }



        }
    
        #region IAppearance Members
        public event EventHandler<TextChangedEventArgs> TextChanged;
        protected void RaiseTextChanged()
        {
            if (this.TextChanged != null)
                this.TextChanged(this, new TextChangedEventArgs(this));
        }

        public string Text
        {
            get { return Description.Name; }
            private set 
            {
                // TODO: implement set
                this.RaiseTextChanged();
            }
        }

        public event EventHandler<ImageChangedEventArgs> ImageChanged;
        protected void RaiseImageChanged()
        {
            if (this.ImageChanged != null)
                this.ImageChanged(this, new ImageChangedEventArgs(this));
        }

        public System.Drawing.Bitmap Image
        {
            get { return PetrelImages.Modules; }
            private set 
            {
                // TODO: implement set
                this.RaiseImageChanged();
            }
        }
        #endregion

        #region IDescriptionSource Members

        /// <summary>
        /// Gets the description of the Workstep
        /// </summary>
        public IDescription Description
        {
            get { return WorkstepDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the Workstep.
        /// Contains Name, Shorter description and detailed description.
        /// </summary>
        public class WorkstepDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private  static WorkstepDescription instance = new WorkstepDescription();
            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static WorkstepDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of Workstep
            /// </summary>
            public string Name
            {
                get { return "Workstep"; }
            }
            /// <summary>
            /// Gets the short description of Workstep
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }
            /// <summary>
            /// Gets the detailed description of Workstep
            /// </summary>
            public string Description
            {
                get { return ""; }
            }

            #endregion
        }
        #endregion


    }
}