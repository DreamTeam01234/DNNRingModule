using System.Collections.Generic;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Ring.Module.DNNRingModule.Models;

namespace Ring.Module.DNNRingModule.Components
{
    interface IPersonalityTestManager
    {
        void CreatePersonalityTest(PersonalityTestAnswer t);
    }

    class PersonalityTestManager : ServiceLocator<IPersonalityTestManager, PersonalityTestManager>, IPersonalityTestManager
    {
        public void CreatePersonalityTest(PersonalityTestAnswer t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<PersonalityTestAnswer>();
                rep.Insert(t);
            }
        }

        protected override System.Func<IPersonalityTestManager> GetFactory()
        {
            return () => new PersonalityTestManager();
        }
    }
}
