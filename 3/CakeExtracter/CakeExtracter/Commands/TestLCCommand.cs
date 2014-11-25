﻿using CakeExtracter.Common;
using LocalConnex;
using System;
using System.ComponentModel.Composition;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class TestLCCommand : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public TestLCCommand()
        {
            IsCommand("testLC");
        }

        public override int Execute(string[] remainingArguments)
        {
            //LocalConnex.Test.Go();

            var lcUtility = new LCUtility(m => Logger.Info(m), m => Logger.Warn(m));
            var accid = "CA6phEyiNYsmxgHn"; // Graebel
            var calls = lcUtility.GetCalls(accid, new DateTime(2014, 11, 18), new DateTime(2014, 11, 18));

            return 0;
        }
    }
}
