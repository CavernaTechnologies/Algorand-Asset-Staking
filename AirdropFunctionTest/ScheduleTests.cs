using AirdropFunction;
using Xunit;

namespace FunctionTest
{
    public class ScheduleTests
    {
        [Fact]
        public void CryptoBunnyHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 15 * * Mon", Function.CryptoBunnyHoldingsAirdropSchedule);
        }

        [Fact]
        public void NanaHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 14 * * Mon,Fri", Function.NanaHoldingsAirdropSchedule);
        }

        [Fact]
        public void ShrimpHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 16 * * Mon,Fri", Function.ShrimpHoldingsAirdropSchedule);
        }

        [Fact]
        public void AlchemonHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 16 * * Sat", Function.AlchemonHoldingsAirdropSchedule);
        }

        [Fact]
        public void AlchemonLiquidityAirdropScheduleTest()
        {
            Assert.Equal("0 0 16 * * Sun", Function.AlchemonLiquidityAirdropSchedule);
        }

        [Fact]
        public void MantisHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 18 * * *", Function.MantisHoldingsAirdropSchedule);
        }

        [Fact]
        public void AlvaHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 16 * * Sun,Tue,Thu,Sat", Function.AlvaHoldingsAirdropSchedule);
        }

        [Fact]
        public void GooseHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 16 * * Mon,Thu", Function.GooseHoldingsAirdropSchedule);
        }

        [Fact]
        public void PyreneesHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 0 * * Mon", Function.PyreneesHoldingsAirdropSchedule);
        }

        [Fact]
        public void GrubHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 19 * * Thu", Function.GrubHoldingsAirdropSchedule);
        }

        [Fact]
        public void HootHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 19 * * Mon", Function.HootHoldingsAirdropSchedule);
        }
    }
}
