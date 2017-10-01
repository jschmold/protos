using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Engine.Utilities.LangHelpers;
namespace EngineTests.Types
{
    [TestClass]
    public class LangHelpers_Tests
    {
        [TestMethod]
        public void DoOrThrow_DoesAction() {
            bool works = false;
            DoOrThrow(() => works = true, new Exception("This should never happen"));
            Assert.IsTrue(works, "Did not call Action instead of Exception");
        }
        [TestMethod]
        public void DoOrThrow_ThrowsException() {
            Assert.ThrowsException<Exception>(() => DoOrThrow(null, new Exception("Yee")));
        }
    }
}
