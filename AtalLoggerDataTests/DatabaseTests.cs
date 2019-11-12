using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AtalLoggerData;
using NUnit.Framework;

namespace AtalLoggerDataTests
{
	[TestFixture]
    public class DatabaseTests
    {
		[Test]
	    public void Test0()
	    {
		    var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		    var db = new AtalLoggerDatabase(Path.Combine(dir, "AtalLoggerData.db"));
		    Assert.IsTrue(db.Check());
	    }
    }
}
