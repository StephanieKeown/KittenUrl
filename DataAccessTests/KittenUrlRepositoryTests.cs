using DataAccess;
using DataAccess.Interfaces;
using DTO;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;
using System.ComponentModel.Design;
using Xunit;

namespace DataAccessTests
{
    public class KittenUrlRepositoryTests
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;
        private IMongoCollection<KittenUrlDto> _urlCollection;
        private IMongoCollection<UniqueKeyDto> _keyCollection;
        private IKittenUrlRepository _repository;

        public KittenUrlRepositoryTests()
        {
            _mongoClient = Substitute.For<MongoClient>();
            _mongoDatabase = Substitute.For<MongoDatabaseBase>();
            _urlCollection = Substitute.For<IMongoCollection<KittenUrlDto>>();
            _keyCollection= Substitute.For<IMongoCollection<UniqueKeyDto>>();
            _repository = Substitute.For<IKittenUrlRepository>();
        }


        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //_repository.testmethod().Returns(true);

            var x = _repository.testmethod();

            //_repository.Received().HandleShortenUrl("ppp");

            Assert.AreEqual(true, x);

            //Assert.Pass();
        }


        //string HandleShortenUrl(string url);
        [Test]
        [Fact]
        public void HandleShortenUrl_CallToCreateRecord_Test()
        {
            //arrange
            string url = "www.testurl.com/testlongurlpath";
            KittenUrlDto dto = new KittenUrlDto();
            _repository.CreateRecord().Returns(dto.Id);
            _repository.UpdateRecord(dto.Id, dto.LongUrl, dto.ShortUrl).Returns(true);
            _repository.testmethod().Returns(true);

            //act
            _repository.HandleShortenUrl(url);
            _repository.testmethod();


            //assert
            //--create record metjod called and id passed in
            //check the kittenurl collection is called
            _repository.Received().HandleShortenUrl("ppp");
            _repository.DidNotReceive().CreateRecord();
            _repository.DidNotReceive().UpdateRecord(dto.Id, dto.LongUrl, dto.ShortUrl);
            _repository.Received().UpdateRecord(dto.Id, dto.LongUrl, dto.ShortUrl);
            _repository.Received().CreateRecord();
            //check a record is returned with an id and empty fields


            Assert.Pass();
        }

        //string FindUrl(string url);  //should return a url type if thats possible.
        [Test]
        public void FindUrl_Test()
        {
            Assert.Pass();
        }

    }
}