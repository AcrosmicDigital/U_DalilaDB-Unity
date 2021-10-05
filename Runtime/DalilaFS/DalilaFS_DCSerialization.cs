using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System.Data;
using System.Security.Cryptography;

namespace U.DalilaDB
{
    // DC for DataContract
    public partial class DalilaFS
    {

        public DataOperation CreateDCResource<TResource>(string resource, TResource file) 
            
            where TResource : class

        {

            var operation = new DataOperation();
            string path; // full path

            if (file == null)
                return operation.Fails(new ArgumentNullException("File to save cant be null"));

            // Validate the Name
            try
            {
                path = ResourceToSystemPath(resource);
            }
            catch (Exception e)
            {
                return operation.Fails(e);
            }


            // Check if the path exist or create it, if cant create return the error
            var locationCreateOperation = CreateLocation(GetResourceLocation(resource));
            if (!locationCreateOperation)
                return locationCreateOperation;


            try
            {
                using (var writer = new FileStream(path, FileMode.Create))
                {

                    DataContractSerializer ser = new DataContractSerializer(typeof(TResource));

                    // Intentar serializar
                    ser.WriteObject(writer, file);

                    return operation.Successful("1");

                }
            }
            catch (Exception e)
            {
                return operation.Fails(e);
            }

        }

        public DataOperation SaveDCResource<TResource>(string resource, TResource file) 
            
            where TResource : class

        {
            var operation = new DataOperation();

            if (ExistResource(resource))
                return operation.Fails(new DuplicateNameException());
            else
                return CreateDCResource(resource, file);

        }

        public DataOperation ReplaceDCResource<TResource>(string resource, TResource file) 
            
            where TResource : class

        {
            var operation = new DataOperation();

            if (!ExistResource(resource))
                return operation.Fails(new Exception());
            else
                return CreateDCResource(resource, file);

        }

        public DataOperation<TResource> ReadDCResource<TResource>(string resource)
            
            where TResource : class

        {
            var operation = new DataOperation<TResource>();
            string path; // full path

            // Validate the Name
            try
            {
                path = ResourceToSystemPath(resource);
            }
            catch (Exception e)
            {
                return operation.Fails(null, e);
            }


            try
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    using (var reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas()))
                    {

                        var ser = new DataContractSerializer(typeof(TResource));

                        // Intentar desserializar
                        var file = (TResource)ser.ReadObject(reader, true);

                        return operation.Successful(file, "1");

                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                return operation.Fails(null, new FileNotFoundException());
            }
            catch (SerializationException)
            {
                return operation.Fails(null, new InvalidCastException());
            }
            catch (Exception e)
            {
                return operation.Fails(null, e);
            }

        }

        public DataOperation<TResource> ReadOrDeleteDCResource<TResource>(string resource)

            where TResource : class

        {
            var operation = ReadDCResource<TResource>(resource);

            if (!operation)
            {
                try
                {
                    throw operation.Error;
                }
                catch (InvalidCastException)
                {
                    DeleteResource(resource);
                }
                catch (Exception)
                {

                }
            }
                

            return operation;
        }

        //--

        public DataOperation CreateEncryDCResource<TResource>(string resource, TResource file)

            where TResource : class

        {

            var operation = new DataOperation();
            string path; // full path

            if (file == null)
                return operation.Fails(new ArgumentNullException("File to save cant be null"));

            // Validate the Name
            try
            {
                path = ResourceToSystemPath(resource);
            }
            catch (Exception e)
            {
                return operation.Fails(e);
            }


            // Check if the path exist or create it, if cant create return the error
            var locationCreateOperation = CreateLocation(GetResourceLocation(resource));
            if (!locationCreateOperation)
                return locationCreateOperation;

            try
            {



                // Create an Aes object
                // with the specified key and IV.
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = _aesKey;

                    // Create the streams used for encryption.
                    using (FileStream msEncrypt = new FileStream(path, FileMode.Create))
                    {

                        // Save the new generated IV.
                        byte[] inputIV = aesAlg.IV;

                        // Write the IV to the FileStream unencrypted.
                        msEncrypt.Write(inputIV, 0, inputIV.Length);

                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV), CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                using (var msStream = new MemoryStream())
                                {
                                    
                                    // Create the data contract serializer of specified type
                                    //Write all data to the stream.
                                    DataContractSerializer ser = new DataContractSerializer(typeof(TResource));
                                    ser.WriteObject(msStream, file);
                                    msStream.Position = 0;

                                    using (StreamReader reader = new StreamReader(msStream))
                                    {
                                        // Serialize the stream
                                        swEncrypt.Write(reader.ReadToEnd());

                                        return operation.Successful("1");
                                    }

                                }
                            }
                        }
                    }
                }


            }
            catch (Exception e)
            {
                return operation.Fails(e);
            }

        }

        public DataOperation SaveEncryDCResource<TResource>(string resource, TResource file)

            where TResource : class

        {
            var operation = new DataOperation();

            if (ExistResource(resource))
                return operation.Fails(new DuplicateNameException());
            else
                return CreateDCResource(resource, file);

        }

        public DataOperation ReplaceEncryDCResource<TResource>(string resource, TResource file)

            where TResource : class

        {
            var operation = new DataOperation();

            if (!ExistResource(resource))
                return operation.Fails(new Exception());
            else
                return CreateDCResource(resource, file);

        }

        public DataOperation<TResource> ReadEncryDCResource<TResource>(string resource)

            where TResource : class

        {
            var operation = new DataOperation<TResource>();
            string path; // full path

            // Validate the Name
            try
            {
                path = ResourceToSystemPath(resource);
            }
            catch (Exception e)
            {
                return operation.Fails(null, e);
            }


            try
            {
                // Create an Aes object
                // with the specified key and IV.
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = _aesKey;

                    // Create a decryptor to perform the stream transform.
                    //ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for decryption.
                    using (FileStream msDecrypt = new FileStream(path, FileMode.Open))
                    {

                        // Create an array of correct size based on AES IV.
                        byte[] outputIV = new byte[aesAlg.IV.Length];

                        // Read the IV from the file.
                        msDecrypt.Read(outputIV, 0, outputIV.Length);
                        aesAlg.IV = outputIV;

                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV), CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                using (var reader = XmlDictionaryReader.CreateTextReader(csDecrypt, new XmlDictionaryReaderQuotas()))
                                {

                                    var ser = new DataContractSerializer(typeof(TResource));

                                    // Intentar desserializar
                                    var file = (TResource)ser.ReadObject(reader, true);

                                    return operation.Successful(file, "1");

                                }
                            }
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                return operation.Fails(null, new FileNotFoundException());
            }
            catch (SerializationException)
            {
                return operation.Fails(null, new InvalidCastException());
            }
            catch (Exception e)
            {
                return operation.Fails(null, e);
            }

        }

        public DataOperation<TResource> ReadOrDeleteEncryDCResource<TResource>(string resource)

            where TResource : class

        {
            var operation = ReadDCResource<TResource>(resource);

            if (!operation)
            {
                try
                {
                    throw operation.Error;
                }
                catch (InvalidCastException)
                {
                    DeleteResource(resource);
                }
                catch (Exception)
                {

                }
            }


            return operation;
        }

        //--

        public static DataOperation<TResource> CloneDCResource<TResource>(TResource file)

            where TResource : class

        {

            var operation = new DataOperation<TResource>();

            if (file == null)
                return operation.Fails(null, new ArgumentNullException("Object to clone cant be null"));


            try
            {
                using (var writer = new MemoryStream())
                {

                    DataContractSerializer ser = new DataContractSerializer(typeof(TResource));

                    // Intentar serializar
                    ser.WriteObject(writer, file);

                    writer.Position = 0;


                    using (var reader = XmlDictionaryReader.CreateTextReader(writer, new XmlDictionaryReaderQuotas()))
                    {

                        // Intentar desserializar
                        var clone = (TResource)ser.ReadObject(reader, true);

                        return operation.Successful(clone, "1");

                    }

                }
            }
            catch (Exception e)
            {
                return operation.Fails(null, e);
            }

        }

    }

}
