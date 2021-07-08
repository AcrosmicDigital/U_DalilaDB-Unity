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

namespace U.DalilaDB
{
    internal partial class DalilaFS
    {

        public DataOperation CreateResource<TResource>(string resource, TResource file) 
            
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

        public DataOperation SaveResource<TResource>(string resource, TResource file) 
            
            where TResource : class

        {
            var operation = new DataOperation();

            if (ExistResource(resource))
                return operation.Fails(new DuplicateNameException());
            else
                return CreateResource(resource, file);

        }

        public DataOperation ReplaceResource<TResource>(string resource, TResource file) 
            
            where TResource : class

        {
            var operation = new DataOperation();

            if (!ExistResource(resource))
                return operation.Fails(new Exception());
            else
                return CreateResource(resource, file);

        }

        public DataOperation<TResource> ReadResource<TResource>(string resource)
            
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

        public DataOperation<TResource> ReadOrDeleteResource<TResource>(string resource)

            where TResource : class

        {
            var operation = ReadResource<TResource>(resource);

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

        public DataOperation<TResourceOut> ReadResource<TResourceRead, TResourceOut>(string resource, Func<TResourceRead, TResourceOut> transformFunc)

             where TResourceRead : class

        {

            var operation = new DataOperation<TResourceOut>();

            var readOperation = ReadResource<TResourceRead>(resource);

            try
            {
                if (readOperation)
                    return operation.Successful(transformFunc(readOperation.Data), readOperation.Message);
                else
                    return operation.Fails(default(TResourceOut), readOperation.Error);
            }
            catch (Exception e)
            {
                return operation.Fails(default(TResourceOut), e);
            }

        }

        public DataOperation<TResourceOut> ReadOrDeleteResource<TResourceRead, TResourceOut>(string resource, Func<TResourceRead, TResourceOut> transformFunc)

             where TResourceRead : class

        {

            var operation = new DataOperation<TResourceOut>();

            var readOperation = ReadOrDeleteResource<TResourceRead>(resource);

            try
            {
                if (readOperation)
                    return operation.Successful(transformFunc(readOperation.Data), readOperation.Message);
                else
                    return operation.Fails(default(TResourceOut), readOperation.Error);
            }
            catch (Exception e)
            {
                return operation.Fails(default(TResourceOut), e);
            }

        }




        public static DataOperation<TResource> CloneResource<TResource>(TResource file)

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
