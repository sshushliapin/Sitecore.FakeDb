using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Sitecore.Data;
using Xunit;

namespace Sitecore.FakeDb.Serialization.Tests
{
    public class SerializedIdToPathResolverTest
    {
        [Fact]
        public void ShouldFindPathForId()
        {
            string filePath = ID.Parse("{108266C2-304B-4AD3-9813-2BE1B88609FF}").FindFilePath("custom");
            
            filePath.Should().NotBeNullOrWhiteSpace();
            File.Exists(filePath).Should().BeTrue();
            Path.GetFileNameWithoutExtension(filePath).ShouldBeEquivalentTo("Item only available in custom serialization folder");
        }

        /// <summary>
        /// This test needs to run very efficiently.
        /// Within a large set of serialized data, looking up data by id could potentially be very slow.
        /// This is because files are organized by their paths and not their id's.
        /// </summary>
        [Fact]
        public void ShouldFindPathForIdInLargeSet()
        {
            string filePath = ID.Parse("{C22B2CD1-EF66-490C-B6F3-A84A59A2B58D}").FindFilePath("largeset");
            
            filePath.Should().NotBeNullOrWhiteSpace();
            File.Exists(filePath).Should().BeTrue();
            Path.GetFileNameWithoutExtension(filePath).ShouldBeEquivalentTo("Auto Item 0");

            string filePath2 = ID.Parse("{0A515E57-2DBD-480E-A0D7-7EE554DD0312}").FindFilePath("largeset");

            filePath2.Should().NotBeNullOrWhiteSpace();
            File.Exists(filePath2).Should().BeTrue();
            Path.GetFileNameWithoutExtension(filePath2).ShouldBeEquivalentTo("Auto Item 9999");
        }
    }
}
