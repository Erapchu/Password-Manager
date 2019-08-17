﻿using Newtonsoft.Json;
using Password_Manager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Password_Manager
{
    class FileProcess
    {

        private static Lazy<FileProcess> _fileProcess = new Lazy<FileProcess>(() => new FileProcess());
        public static FileProcess Instance { get { return _fileProcess.Value; } }

        private readonly string PathToMainFile;
        private readonly int DefaultRandomSize;
        public FileProcess()
        {
            PathToMainFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\testdata.dat";
            DefaultRandomSize = 20;
        }

        /// <summary>
        /// Write file to my documents
        /// </summary>
        /// <param name="account">List of instances AccountData</param>
        /// <returns></returns>
        public bool WriteFile(Account account)
        {
            try
            {
                string forFile = JsonConvert.SerializeObject(account);
                int keyEncrypt = new Random().Next(1, DefaultRandomSize);
                string encryptedForFile = Encryption.Process(forFile, keyEncrypt);

                using (BinaryWriter bw = new BinaryWriter(File.Open(PathToMainFile, FileMode.Create)))
                {
                    bw.Write(keyEncrypt);
                    bw.Write(encryptedForFile);
                }

                /*using (BinaryWriter bw = new BinaryWriter(File.Open(PathToMainFile, FileMode.Create)))
                {
                    //Header Ключ для шифрования и зашифрованный пароль
                    bw.Write(Encryption.KEY);
                    bw.Write(Encryption.Process(correctPass));

                    //Зашифрованные данные
                    foreach (AccountData data in datas)
                    {
                        bw.Write(Encryption.Process(data.Name));
                        bw.Write(Encryption.Process(data.Login));
                        bw.Write(Encryption.Process(data.Password));
                        bw.Write(Encryption.Process(data.Other));
                    }
                }*/
                return true;
            }
            catch
            {
                MessageBox.Show("Problems while write file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Read file data
        /// </summary>
        /// <returns></returns>
        public Account ReadFile()
        {
            try
            {
                int keyDecrypt;
                string encryptedFromFile;
                string fromFile;

                using(BinaryReader br = new BinaryReader(File.Open(PathToMainFile, FileMode.Open)))
                {
                    keyDecrypt = br.ReadInt32();
                    encryptedFromFile = br.ReadString();
                }

                fromFile = Encryption.Process(encryptedFromFile, keyDecrypt);
                Account account = new Account();
                account = JsonConvert.DeserializeObject<Account>(fromFile);
                return account;

                /*using (BinaryReader br = new BinaryReader(File.Open(PathToMainFile, FileMode.Open)))
                {
                    br.BaseStream.Position = offsetToRead;
                    AccountData data;
                    while (br.PeekChar() != -1)
                    {
                        data = new AccountData
                        {
                            Name = br.ReadString(),
                            Login = br.ReadString(),
                            Password = br.ReadString(),
                            Other = br.ReadString()
                        };
                        accountDatas.Add(data);
                    }
                }*/
            }
            catch (FileNotFoundException)
            {
                File.Create(PathToMainFile);
                MessageBox.Show("New file have been created", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }
            catch
            {
                MessageBox.Show("File is corrupt", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /*/// <summary>
        /// Read password
        /// </summary>
        /// <returns></returns>
        public string ReadPassword()
        {
            try
            {
                string pass;
                using (BinaryReader br = new BinaryReader(File.Open(PathToMainFile, FileMode.Open)))
                {
                    //Получение ключа для дешифрования
                    Encryption.KEY = br.ReadInt32();
                    //Чтение пароля
                    pass = Encryption.Process(br.ReadString());
                    //Сохранение смещения позиции
                    offsetToRead = br.BaseStream.Position;
                }
                return pass;
            }
            catch (FileNotFoundException)
            {
                File.Create(PathToMainFile);
                MessageBox.Show("New file have been created", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }
            catch
            {
                MessageBox.Show("Header of file is corrupt", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }*/
    }
}
