using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Neurio.Client.Tests
{
    [TestFixture]
    public class CoreTests : TestBase
    {

        [Test]
        public void Login()
        {

            DefaultClient.Login(System.Configuration.ConfigurationManager.AppSettings["Username"],
                System.Configuration.ConfigurationManager.AppSettings["Password"]).ContinueWith(
                    t =>
                    {
                        Assert.IsTrue(t.Result.Success, t.Result.Message);
                    }).Wait();
        }

        [Test]
        public void CurrentUser()
        {
            base.Login().ContinueWith(t =>
            {
                Assert.IsTrue(t.Result.Success, t.Result.Message);
                DefaultClient.CurrentUser().ContinueWith(u =>
                {
                    Assert.IsTrue(u.Result.Success, u.Result.Message);
                    Assert.IsNotNull(u.Result.Email, u.Result.Message);
                }).Wait();
            }).Wait();
        }

        [Test]
        public void Appliances()
        {

            base.Login().ContinueWith(t =>
            {
                Assert.IsTrue(t.Result.Success, t.Result.Message);
                DefaultClient.CurrentUser().ContinueWith(u =>
                {
                    Assert.IsTrue(u.Result.Success, u.Result.Message);
                    foreach (var l in u.Result.Locations)
                    {
                        DefaultClient.Appliances(l.Id).ContinueWith(v =>
                        {
                            Assert.IsTrue(v.Result.Success, v.Result.Message);
                            Assert.IsNotNull(v.Result.Appliances, v.Result.Message);
                        }).Wait();

                    }
                }).Wait();
            }).Wait();
        }

        [Test]
        public void Sensor_SampleStats()
        {

            base.Login().ContinueWith(t =>
            {
                Assert.IsTrue(t.Result.Success, t.Result.Message);
                DefaultClient.CurrentUser().ContinueWith(u =>
                {
                    Assert.IsTrue(u.Result.Success, u.Result.Message);
                    foreach (var l in u.Result.Locations)
                    {
                        foreach (var s in l.Sensors)
                        {
                            DefaultClient.SensorStatsSamples(s.Id, DateTimeOffset.MinValue, DateTimeOffset.MaxValue).ContinueWith(ss =>
                            {
                                Assert.IsNotNull(ss.Result);
                            }).Wait();
                        }
                    }
                }).Wait();
            }).Wait();
        }


        [Test]
        public void Sensor_SampleLive()
        {
            base.Login().ContinueWith(t =>
            {
                Assert.IsTrue(t.Result.Success, t.Result.Message);
                DefaultClient.CurrentUser().ContinueWith(u =>
                {
                    Assert.IsTrue(u.Result.Success, u.Result.Message);
                    foreach (var l in u.Result.Locations)
                    {
                        foreach (var s in l.Sensors)
                        {
                            DefaultClient.SensorLiveSamples(s.Id, DateTimeOffset.MinValue).ContinueWith(ss =>
                            {
                                Assert.IsNotNull(ss.Result);
                            }).Wait();
                        }
                    }
                }).Wait();
            }).Wait();
        }

        [Test]
        public void Location_Events()
        {
            base.Login().ContinueWith(t =>
            {
                Assert.IsTrue(t.Result.Success, t.Result.Message);
                DefaultClient.CurrentUser().ContinueWith(u =>
                {
                    Assert.IsTrue(u.Result.Success, u.Result.Message);
                    foreach (var l in u.Result.Locations)
                    {
                        DefaultClient.LocationEvents(l.Id, DateTimeOffset.MinValue, DateTimeOffset.MaxValue).ContinueWith(ss =>
                        {
                            Assert.IsNotNull(ss.Result);
                        }).Wait();

                    }
                }).Wait();
            }).Wait();
        }

        [Test]
        public void Location_Stats()
        {
            base.Login().ContinueWith(t =>
            {
                Assert.IsTrue(t.Result.Success, t.Result.Message);
                DefaultClient.CurrentUser().ContinueWith(u =>
                {
                    Assert.IsTrue(u.Result.Success, u.Result.Message);
                    foreach (var l in u.Result.Locations)
                    {
                        DefaultClient.LocationStats(l.Id, DateTimeOffset.MinValue, DateTimeOffset.MaxValue).ContinueWith(ss =>
                        {
                            Assert.IsNotNull(ss.Result);
                        }).Wait();

                    }
                }).Wait();
            }).Wait();
        }
    }
}
