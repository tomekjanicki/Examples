using FluentAssertions;
using Moq;

namespace Test.Presentation;

public class UnitTests
{
    private readonly Mock<IDependency> _dependencyMock;
    private readonly Service _service;

    public UnitTests()
    {
        _dependencyMock = new Mock<IDependency>();
        _service = new Service(_dependencyMock.Object);
    }

    [Fact]
    public void GoodStateBasedUnitTestWithStub()
    {
        _dependencyMock.Setup(dependency => dependency.GetString1(It.IsAny<int>())).Returns("a");
        _dependencyMock.Setup(dependency => dependency.GetString2(It.IsAny<int>())).Returns("b");
        var result = _service.GetSth(5, 6);

        result.Should().Be("ab");
    }

    [Fact]
    public void GoodInteractionBasedUnitTestWithMock()
    {
        _dependencyMock.Setup(dependency => dependency.GetString1(It.IsAny<int>())).Returns("a");
        _dependencyMock.Setup(dependency => dependency.GetString2(It.IsAny<int>())).Returns("b");
        _service.ExecuteSth(5);

        _dependencyMock.Verify(dependency => dependency.PublishEvent(It.IsAny<int>())); //verify only key behaviour, not all
    }

    [Fact]
    public void BadStateBasedUnitTestWithMock()
    {
        _dependencyMock.Setup(dependency => dependency.GetString1(It.IsAny<int>())).Returns("a");
        _dependencyMock.Setup(dependency => dependency.GetString2(It.IsAny<int>())).Returns("b");
        _service.GetSth(5, 6);

        _dependencyMock.Verify(dependency => dependency.GetString1(It.IsAny<int>()));
        _dependencyMock.Verify(dependency => dependency.GetString2(It.IsAny<int>()));
    }

    [Fact]
    public void BadStateBasedUnitTestWithMockAndStub()
    {
        _dependencyMock.Setup(dependency => dependency.GetString1(It.IsAny<int>())).Returns("a");
        _dependencyMock.Setup(dependency => dependency.GetString2(It.IsAny<int>())).Returns("b");
        var result = _service.GetSth(5, 6);

        result.Should().Be("ab");
        _dependencyMock.Verify(dependency => dependency.GetString1(It.IsAny<int>()));
        _dependencyMock.Verify(dependency => dependency.GetString2(It.IsAny<int>()));
    }

    [Fact]
    public void BadInteractionBasedUnitTestWithMock()
    {
        _dependencyMock.Setup(dependency => dependency.GetString1(It.IsAny<int>())).Returns("a");
        _dependencyMock.Setup(dependency => dependency.GetString2(It.IsAny<int>())).Returns("b");
        _service.ExecuteSth(5);

        _dependencyMock.Verify(dependency => dependency.PublishEvent(It.IsAny<int>()));
        _dependencyMock.Verify(dependency => dependency.LogSth(It.IsAny<int>()));
        _dependencyMock.Verify(dependency => dependency.GetString1(It.IsAny<int>()));
        _dependencyMock.Verify(dependency => dependency.GetString2(It.IsAny<int>()));
    }

    public sealed class Service
    {
        private readonly IDependency _dependency;

        public Service(IDependency dependency)
        {
            _dependency = dependency;
        }

        public string GetSth(int p1, int p2)
        {
            var r1 = _dependency.GetString1(p1);
            var r2 = _dependency.GetString2(p2);
            if (r1 is not null && r2 is not null)
            {
                return $"{r1}{r2}";
            }

            return string.Empty;
        }

        public void ExecuteSth(int p)
        {
            var r1 = _dependency.GetString1(p);
            var r2 = _dependency.GetString2(p);
            if (r1 is not null && r2 is not null)
            {
                _dependency.LogSth(p);
                _dependency.PublishEvent(p);

                return;
            }

            _dependency.LogSth(p);
        }
    }

    public interface IDependency
    {
        string? GetString1(int inputParam);

        string? GetString2(int inputParam);

        void PublishEvent(int inputParam);

        void LogSth(int param);
    }
}