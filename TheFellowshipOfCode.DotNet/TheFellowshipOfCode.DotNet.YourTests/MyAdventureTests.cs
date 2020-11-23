using System.Threading.Tasks;
using FluentAssertions;
using HTF2020.Contracts.Requests;
using TheFellowshipOfCode.DotNet.YourAdventure;
using Xunit;

namespace TheFellowshipOfCode.DotNet.YourTests
{
    public class MyAdventureTests
    {
        [Fact]
        public async Task MyAdventure_CreateParty_Should_Create_Two_Party_Members()
        {
            // Arrange
            var adventure = new MyAdventure();
            var request = new CreatePartyRequest
            {
                MembersCount = 2
            };

            // Act
            var response = await adventure.CreateParty(request);

            // Assert
            response.Name.Should().Be("My Party");
            response.Members.Should().HaveCount(2);
            response.Members.Should().Contain(x => x.Name == "Member 1");
            response.Members.Should().Contain(x => x.Name == "Member 2");
        }
    }
}