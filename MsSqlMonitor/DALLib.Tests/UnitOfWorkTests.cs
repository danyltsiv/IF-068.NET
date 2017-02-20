using System;
using System.Linq;
using DALLib.EF;
using DALLib.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;

namespace DALLib.Tests
{
    [TestClass]
    public class UnitOfWorkTests
    {
        Mock<MsSqlMonitorEntities> mockContext;
        Mock<DbSet<Instance>> mockSet;
        IQueryable<Instance> instances;
        List<Instance> instancesList;

        [TestInitialize()]
        public void Initialize()
        {
            mockContext = new Mock<MsSqlMonitorEntities>();
            mockSet = new Mock<DbSet<Instance>>();
            mockContext.Setup(c => c.Set<Instance>()).Returns(mockSet.Object);

            instancesList = new List<Instance>
            {
                new Instance { Id = 1, Login = "Login1", Password = "Password1", InstanceName = "Inst1", ServerName = "Server1", IsDeleted = false, Status = InstanceStatus.Online, Authentication = AuthenticationType.Windows },
                new Instance { Id = 2, Login = "Login2", Password = "Password2", InstanceName = "Inst2", ServerName = "Server2", IsDeleted = false, Status = InstanceStatus.Online, Authentication = AuthenticationType.Windows  },
                new Instance { Id = 3, Login = "Login3", Password = "Password3", InstanceName = "Inst3", ServerName = "Server3", IsDeleted = false, Status = InstanceStatus.Online, Authentication = AuthenticationType.Windows  }
            };//тут
            instances = instancesList.AsQueryable();
            mockSet.As<IQueryable<Instance>>().Setup(m => m.Provider).Returns(instances.Provider);
            mockSet.As<IQueryable<Instance>>().Setup(m => m.Expression).Returns(instances.Expression);
            mockSet.As<IQueryable<Instance>>().Setup(m => m.ElementType).Returns(instances.ElementType);
            mockSet.As<IQueryable<Instance>>().Setup(m => m.GetEnumerator()).Returns(instances.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                    .Returns<object[]>(ids => instances
                    .FirstOrDefault(d => d.Id == (int)ids[0]));
        }

        [TestMethod]
        public void InstanceCreate()
        {

            using (var unitOfWork = new UnitOfWork(mockContext.Object, null, null))
            {
                Instance instance = new Instance
                {
                    Id = 3,
                    Login = "Login3",
                    Password = "Password3",
                    InstanceName = "Inst3",
                    ServerName = "Server3",
                    IsDeleted = false,
                    Status = InstanceStatus.Online,
                    Authentication = AuthenticationType.Windows
                };

                unitOfWork.Instances.Create(instance);
                unitOfWork.Save();
            }
            mockSet.Verify(m => m.Add(It.IsAny<Instance>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void InstanceGet()
        {

            Instance instance;
            using (var unitOfWork = new UnitOfWork(mockContext.Object, null, null))
            {
                instance = unitOfWork.Instances.Get(1);
            }

            mockSet.Verify(m => m.Find(It.IsAny<int>()), Times.Once());
            Assert.AreEqual(instance.ServerName, "Server1");
        }

        [TestMethod]
        public void InstanceDelete()
        {
            //mockSet.Setup(m => m.Remove(It.IsAny<Instance>())).Verifiable();
            //mockSet.Setup(m => m.Remove(It.IsAny<User>()))
            //        .Callback((User g) =>
            //        {
            //            usersList.Remove(g);
            //        });

            //int usersCount;
            using (var unitOfWork = new UnitOfWork(mockContext.Object, null, null))
            {
                unitOfWork.Instances.Delete(1);
                unitOfWork.Save();
                //usersCount = unitOfWork.Users.GetAll().Count();
            }

            mockSet.Verify(m => m.Find(It.IsAny<int>()), Times.Once());
            mockSet.Verify(m => m.Remove(It.IsAny<Instance>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            //Assert.AreEqual(usersCount, 2);
        }

        [TestMethod]
        public void InstanceFind()
        {
            Instance instance;
            using (var unitOfWork = new UnitOfWork(mockContext.Object, null, null))
            {
                instance = unitOfWork.Instances.FindBy(g => g.ServerName.Length > 0).FirstOrDefault();
            }

            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void InstanceGetAll()
        {
            List<Instance> instances;
            using (var unitOfWork = new UnitOfWork(mockContext.Object, null, null))
            {
                instances = unitOfWork.Instances.GetAll().ToList();
            }

            Assert.AreEqual(3, instances.Count());
            Assert.AreEqual("Server1", instances[0].ServerName);
            Assert.AreEqual("Server2", instances[1].ServerName);
            Assert.AreEqual("Server3", instances[2].ServerName);
        }

        //[TestMethod]
        //public void InstanceUpdate()
        //{
        //    Instance instance = new Instance
        //    {
        //        Id = 3,
        //        Login = "LoginNew",
        //        Password = "PasswordNew",
        //        InstanceName = "InstNew",
        //        ServerName = "ServerNew",
        //        IsDeleted = false,
        //        Status = InstanceStatus.Online,
        //        Authentication = AuthenticationType.Windows
        //    };
        //    Instance newInstance;

        //    using (var unitOfWork = new UnitOfWork(mockContext.Object, null))
        //    {
        //        unitOfWork.Instances.Update(instance);
        //        unitOfWork.Save();
        //        newInstance = unitOfWork.Instances.Get(instance.Id);
        //    }

        //    Assert.AreEqual(newInstance.Login, instance.Login);
        //}
    }
}
