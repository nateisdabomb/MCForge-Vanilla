﻿/*
Copyright 2011 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utils;
namespace MCForge.Commands
{
    public class CmdWhisper : ICommand
    {
        public string Name { get { return "Whisper"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            Player who = Player.Find(args[0]);
            if (who == null) { p.SendMessage("Player not found!"); return; }
            if (who == p) { p.SendMessage("Cannot whisper to yourself!"); return; }

            p.ExtraData.CreateIfNotExist("Whispering", false);
            p.ExtraData.CreateIfNotExist("WhisperTo", null);

            if (!(bool)p.ExtraData["Whispering"])
            {
                p.SendMessage("All messages will be sent to " + who.Username);
                p.ExtraData["Whispering"] = true;
                p.ExtraData["WhisperTo"] = who;
                return;
            }
            else
            {
                p.SendMessage("Whisper mode disabled!");
                p.ExtraData["Whispering"] = false;
                p.ExtraData["WhisperTo"] = null;
                return;
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/whisper <player> - enables whisper mode");
            p.SendMessage("All messages will be sent to <player>");
        }

        public void Initialize()
        {
            Command.AddReference(this, "whisper");
        }
    }
}