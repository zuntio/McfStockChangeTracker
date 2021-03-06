﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using McfStockChangeTracker.CsvHelperClassMaps;
using McfStockChangeTracker.Models;
using McfStockChangeTracker.Options;

namespace McfStockChangeTracker.Services
{
    public class FileIOService
    {
        private static readonly string _myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static readonly string _appRootFolder = Path.Combine(_myDocuments, "StockChangeTracker");
        private static readonly string _configFolder = Path.Combine(_appRootFolder, "config");
        private readonly string _inputFolder = Path.Combine(_appRootFolder, "input");
        private readonly string _outputFolder = Path.Combine(_appRootFolder, "output");
        private readonly string _storeConfigFile = Path.Combine(_configFolder, "credentials.ini");
        private readonly string _userConfigFile = Path.Combine(_configFolder, "users.ini");

        public bool WasInitialSetup { get; private set; }

        public string AppRootFolder => _appRootFolder;
        public string OutputFolder => _outputFolder;
        public string StoreConfigFile => _storeConfigFile;
        public string UserConfigFile => _userConfigFile;

        public FileIOService()
        {
            WasInitialSetup = false;
            CheckAndCreateFoldersAndConfigurationFiles();
        }

        public McfStockChangeTrackerApiOptions GetCredentials()
        {
            var text = File.ReadAllLines(_storeConfigFile, Encoding.UTF8);
            var firstLine = text.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(firstLine))
                throw new Exception($"Verkkokaupan API-tunnukset puuttuvat. Lisää ne tiedostoon {_storeConfigFile}");
            var credentials = firstLine.Split(":");
            if (credentials.Length != 3)
                throw new Exception($"Ohjelma tarvitsee kaikki kolme asiaa: Verkkokaupan nimen, käyttäjätunnuksen ja api-avaimen. Yksi tai useampi puuttuu. Tarkista {_storeConfigFile}");
            return new McfStockChangeTrackerApiOptions
            {
                StoreName = credentials[0],
                ApiUser = credentials[1],
                ApiKey = credentials[2]
            };
        }

        public McfStockChangeTrackerUserOptions GetUsers()
        {
            var users = new Dictionary<string, string>();
            var text = File.ReadAllLines(_userConfigFile, Encoding.UTF8);
            var filteredLines = text.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var splitLines = filteredLines.Select(x => x.Split(":")).Where(x => x.Length == 2).ToList();
            splitLines.ForEach(x =>
            {
                try
                {
                    users.Add(x[0].Trim(), x[1].Trim());
                }
                catch (Exception ex)
                {
                    throw new Exception($"Sama käyttäjätunnus syötetty useaan kertaan. Tarkista {_userConfigFile}");
                }
            });

            return new McfStockChangeTrackerUserOptions
            {
                Users = users
            };

        }

        private void CheckAndCreateFoldersAndConfigurationFiles()
        {
            if (!Directory.Exists(_appRootFolder))
            {
                WasInitialSetup = true;
                Directory.CreateDirectory(_appRootFolder);
            }
            if (!Directory.Exists(_configFolder))
            {
                WasInitialSetup = true;
                Directory.CreateDirectory(_configFolder);
            }
            if (!Directory.Exists(_inputFolder))
            {
                WasInitialSetup = true;
                Directory.CreateDirectory(_inputFolder);
            }
            if (!Directory.Exists(_outputFolder))
            {
                WasInitialSetup = true;
                Directory.CreateDirectory(_outputFolder);
            }
            if (!File.Exists(_storeConfigFile))
            {
                WasInitialSetup = true;
                File.CreateText(_storeConfigFile);
            }
            if (!File.Exists(_userConfigFile))
            {
                WasInitialSetup = true;
                File.CreateText(_userConfigFile);
            }
        }

        public async Task WriteToCsv(string filename, List<StockChangeDto> data)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<StockChangeDtoClassMap>();
                csv.Configuration.Delimiter = ";";
                await csv.WriteRecordsAsync(data);
                await csv.FlushAsync();
            }

            await File.WriteAllTextAsync(Path.Combine(_outputFolder, filename), sb.ToString(), Encoding.UTF8);
        }

    }
}
