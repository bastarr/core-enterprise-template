using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;

namespace Acme.Business.Tests
{
    public abstract class BaseUnitTest
    {
        public static void Init()
        {
        }

        public static List<T> GetMockDataSet<T>(MockDataEntity dataSet) where T : class
        {
            var fileName = Enum.GetName(typeof(MockDataEntity), dataSet);
            var path = $"{Directory.GetCurrentDirectory()}/TestData/{fileName}.json";

            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<T> data = (List<T>)serializer.Deserialize(file, typeof(List<T>));

                return data;
            }
        }

        public static Mock<DbSet<T>> GetMockDbSet<T>(MockDataEntity dataSet) where T : class
        {
            var source = GetMockDataSet<T>(dataSet);
            var queryable = source.AsQueryable<T>();

            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>((entity) => source.Add(entity));
            mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>((T entity) => source.Remove(entity));

            return mockSet;
        }
    }
}