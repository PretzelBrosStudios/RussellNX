﻿using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RussellNX
{
    public partial class ProjectSettings : Form
    {
        private const string SettingsTemplate =
@"{
    ""id"": ""3a5af38c-757d-41ae-98c0-5d4b09e14e6a"",
    ""modelName"": ""GMSwitchOptions"",
    ""mvc"": ""1.0"",
    ""name"": ""Switch"",
    ""option_switch_check_nsp_publish_errors"": {CHECKNSP},
    ""option_switch_enable_fileaccess_checking"": {FILEACCESS},
    ""option_switch_enable_nex_libraries"": {NEXLIBS},
    ""option_switch_interpolate_pixels"": {INTERPOLATE},
    ""option_switch_project_nmeta"": ""{NMETAPATH}"",
    ""option_switch_scale"": {SCALE},
    ""option_switch_texture_page"": ""{TPAGESIZE}""
}";
        private static string FilePath = "";
        public ProjectSettings()
        {
            InitializeComponent();
        }

        public ProjectSettings(string projpath) : this()
        {
            var fpath = Path.GetDirectoryName(projpath) + Path.DirectorySeparatorChar + "options" + Path.DirectorySeparatorChar + "switch" + Path.DirectorySeparatorChar + "options_switch.yy";
            ParseFile(fpath);
            labelRndQuote.Text += GenRndQuote();
        }

        public void ParseFile(string path)
        {
            // Yes, I know that this is ugly. But this way we don't have to rely on JSON libraries.

            string[] settings = File.ReadAllLines(path);
            checkNSPPublish.Checked = settings[5].Replace("    \"option_switch_check_nsp_publish_errors\": ", string.Empty) == "true,";
            checkFileAccessLog.Checked = settings[6].Replace("    \"option_switch_enable_fileaccess_checking\": ", string.Empty) == "true,";
            checkNEXLibs.Checked = settings[7].Replace("    \"option_switch_enable_nex_libraries\": ", string.Empty) == "true,";
            checkInterpolation.Checked = settings[8].Replace("    \"option_switch_interpolate_pixels\": ", string.Empty) == "true,";
            textBoxNmeta.Text = settings[9].Replace("    \"option_switch_project_nmeta\": \"", string.Empty).TrimEnd(new char[] { '"', ',' });
            string scale = settings[10].Replace("    \"option_switch_scale\": ", string.Empty);
            if (scale == "0,")
            {
                radioFullScale.Checked = false;
                radioKeepAspect.Checked = true;
            }
            else if (scale == "1,")
            {
                radioFullScale.Checked = true;
                radioKeepAspect.Checked = false;
            }
            else
            {
                MessageBox.Show("Error when parsing scale!");
            }

            string tpageSize = settings[11].Replace("    \"option_switch_texture_page\": ", "").Replace("\"", "");
            foreach (var item in comboTPageSize.Items)
            {
                if (tpageSize == item.ToString())
                {
                    comboTPageSize.SelectedItem = item;
                    break;
                }
            }

            FilePath = path;
        }

        private void SaveSettingsBtn_Click(object sender, EventArgs e)
        {
            string settings = SettingsTemplate;
            settings = settings.Replace("{CHECKNSP}", checkNSPPublish.Checked ? "true" : "false");
            settings = settings.Replace("{FILEACCESS}", checkFileAccessLog.Checked ? "true" : "false");
            settings = settings.Replace("{NEXLIBS}", checkNEXLibs.Checked ? "true" : "false");
            settings = settings.Replace("{INTERPOLATE}", checkInterpolation.Checked ? "true" : "false");
            settings = settings.Replace("{NMETAPATH}", textBoxNmeta.Text);
            if (radioFullScale.Checked && !radioKeepAspect.Checked)
                settings = settings.Replace("{SCALE}", "1");
            else
                settings = settings.Replace("{SCALE}", "0");
            settings = settings.Replace("{TPAGESIZE}", comboTPageSize.SelectedItem.ToString());

            File.WriteAllText(FilePath, settings, Encoding.UTF8);

            Close();
        }

        private string GenRndQuote()
        {
            string[] quotes =
            {
                "If you feel tired, go outside for a walk.",
                "May contain piracy.",
                "2.3 support wen",
                "animals r cute",
                "why do I suck at making UI design?",
                "remember to take a break every 30 minutes.",
                "JSON ABOVE ALL!!@@@!!!!!11111one",
                "Stay safe, stay home.",
                "australia's fake lmao",
                "lojical sucks"
            };

            return quotes[new Random().Next(0, quotes.Length - 1)];
        }
    }
}