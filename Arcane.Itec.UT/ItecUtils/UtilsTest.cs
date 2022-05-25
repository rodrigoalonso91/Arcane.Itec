using NUnit.Framework;
using Arcane.Itec.ItecUtils;

namespace Arcane.Itec.UT.ItecUtils
{
    public class UtilsTest
    {
        [Test]
        public void GetEfectivity_CorrectOperation()
        {
            int totalClients = 500;
            int myClients = 100;
            double expected = 20;

            var efectivity = Utils.GetEfectivity(totalClients, myClients);

            Assert.AreEqual(expected, efectivity);
        }

        [Test]
        public void GetEfectivity_Should_ReturnDouble()
        {
            int totalClients = 500;
            int myClients = 100;

            var efectivity = Utils.GetEfectivity(totalClients, myClients);

            Assert.True(efectivity.GetType() == typeof(double));
        }
    }
}
