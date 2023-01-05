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
            Assert.Equal("0 0 16 * * Mon", Function.ShrimpHoldingsAirdropSchedule);
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

        [Fact]
        public void BuzzyBeeHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 19 * * Fri", Function.BuzzyBeeHoldingsAirdropSchedule);
        }

        [Fact]
        public void BrontoHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 21 * * Wed", Function.BrontoHoldingsAirdropSchedule);
        }

        [Fact]
        public void BiteHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 0 * * Thu", Function.BiteHoldingsAirdropSchedule);
        }

        [Fact]
        public void YarnHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 30 23 * * Thu", Function.YarnHoldingsAirdropSchedule);
        }

        [Fact]
        public void CreamHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 20 20 * * Sun", Function.CreamHoldingsAirdropSchedule);
        }

        [Fact]
        public void BallstarHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 22 * * Mon", Function.BallstarHoldingsAirdropSchedule);
        }

        [Fact]
        public void GrowHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 1 * * Mon", Function.GrowHoldingsAirdropSchedule);
        }

        [Fact]
        public void MagoHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 15 * * Mon", Function.MagoHoldingsAirdropSchedule);
        }

        [Fact]
        public void AlgoleaguesHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 23 * * Sun", Function.AlgoleaguesHoldingsAirdropSchedule);
        }

        [Fact]
        public void BlopHoldingsAirdropScheduleTest()
        {
            Assert.Equal("0 0 13 * * *", Function.BlopHoldingsAirdropSchedule);
        }
    }
}
