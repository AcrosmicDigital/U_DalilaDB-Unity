using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using U.DalilaDB;
using UnityEngine;

namespace DalilaFsTests
{
    public static class DataSource_SacrumFileSystem
    {

        public static string[] validResources = new string[]
            {
                "/cat", // Cant be null or empty, Must start with '/', Cant end with '/', Only can contain numbers, letters, and '/', Cant have more than one '/' together
                "/cats/canary",
                "/dog",
                "/bull/dog",
                "/bird",
                "/birds/bison",
                "/boar/buffalo",
                "/camel/cat/calf",
                "/bee",
                "/beavers",
                "/beaver/canarys",
                "/beaver/canary/chickens",
                "/beaver/canary/chicken/cows",
                "/beaver/canary/chicken/cow/donkey",
                "/gooses",
                "/goose/gorillas",
                "/goose/gorilla/goldfishs",
                "/goose/gorilla/goldfish/falcon",
                "/cat34/4234233",
                "/dog22",
                "/bull222/dog33",
                "/bird65",
                "/bird66/bison44",
                "/cats/canary.lol",  // Cant have '.' , but no together, and cant end with it
                "/dog.bin",
                "/bull/dog.dev.bin",
                "/bird.dev",
                "/birds/bison.prod",
                "/boars/buffalo.a",
                "/camel/cat/calf.66",
                "/bee.w33.3eee.3.4",
                "/.test",
                "/.git/dev",
                "/no.one/.test",
                "/err.dev/no.one/.test",
                "/TheNewOrder/66",
                "/Last.Order/.dev/.test",
                "/New.Order/dev/Test.dev",
            };

        public static string[] invalidResources = new string[]
            {
                null,  // Cant be null
                "",    // Cant be empty
                "/cat/canary/", // Cant end with '/'
                "/",
                "/dog/",
                "/bull/dog/",
                "/bird/",
                "/bird/bison/",
                " ",  // No spaces
                "/beaver /canary",
                "/beaver/can ary/chicken",
                " /beaver/canary/chicken/cow",
                 "/beaver/can ary/chicken ",
                " /beaver/canary/chicken/ cow",
                "$", // No special charanters
                "/bea!ver",
                "/beaver))/canary",
                "/beaver/can.ary/ch,icken",
                "/Goose/gorilla./goldfish",
                "/gOose/gorilla#/goldfish/falcon",
                "/cat34/423S4233#",
                "goose",// Must start with '/'
                "goose/gorilla",
                "goose/gorilla/goldfish",
                "//beaver",  // Cant have more than one '/' together
                "/beaver///canary",
                "/beaver//canary/chicken",
                "//beaver/canary//chicken//cow",
                "/cat/canary..lol",  // Cant have '.' in the last part, but no together, and cant end with it
                "/dog.bin.",
                "/bull/dog.dev..bin",
                "/bird.dev.",
                "/bird/bison.prod.",
                "/boar/..buffalo.a",
                "/camel/cat/calf.6..6",
                "/bee.w33.3eee...3.4.",
                "./goose",
                "/goo.se./gorilla",
                "/goose/go.ril.la./goldfish",
                "dog./goose",
                "/goo.se./gorilla",
                "/goose/go.ril.la./goldfish",
            };

        public static string[] validLocations = new string[]
            {
                "/", // Cant be null or empty, Must start with '/', Must end with '/', Only can contain numbers, lowecase letters, and '/', Cant have more than one '/' together
                "/apple/",
                "/apple/apricot/",
                "/avocado/",
                "/bull/avocado/",
                "/banana/",
                "/banana/cherry/",
                "/fig/grapes/",
                "/lemon/lime/mango/",
                "/orange/",
                "/blackberry/",
                "/blackberry/apricot/",
                "/blackberry/apricot/lime/",
                "/blackberry/apricot/lime/peach/",
                "/blackberry/apricot/lime/peach/pineapple/",
                "/tomato/",
                "/tomato/watermelon/",
                "/tomato/watermelon/cauliflower/",
                "/tomato/watermelon/cauliflower/celery/",
                "/celery34/4234233/",
                "/cucumber22/",
                "/lettuce222/onion33/",
                "/peas65/",
                "/peas66/onion44/",
                "/beetroot/beetroot/beetroot/",
                "/.test/",
                "/.git/dev/",
                "/no.one/.test/",
                "/err.dev/no.one/.test/",
                "/TheNewOrder/66/",
                "/Last.Order/.dev/.test/",
                "/New.Order/dev/Test.dev/",
            };

        public static string[] invalidlocations = new string[]
            {
                null,  // Cant be null
                "",    // Cant be empty
                "/orange", // Must end with '/'
                "/blackberry",
                "/blackberry/apricot",
                "/blackberry/apricot/lime",
                " ",    // No spaces
                "/blackberry/apr icot/lime/peach/",
                " /blackberry/apricot/lime/peach/pineapple/",
                "/tomato/ ",
                "/tomato/ watermelon/",
                "#",   // No special charanters
                "/bull/avo#cado/",
                "/banana./",
                "/banana/cherr,y/",
                "/fig/grap=es/",
                "/pea.s65./",
                "/peas66./onio.n44/",
                "/beetroot/beetroot./beetr.oot/",
                "/tomSato#/",  
                "/toma#to/Zwatermelon/",
                "/toma,to/watWermelon/cauliflower/",
                "fig/grapes/",   // must start with '/'
                "lemon/lime/mango/",
                "orange/",
                "//banana/cherry/",  // Cant have more than one '/' together
                "//",
                "/fig//grapes/",
                "/lemon/lime/mango//",
                "//orange/",
            };

        public static string[] validLocationsPrev = new string[]
            {
                "/", // Cant be null or empty, Must start with '/', Must end with '/', Only can contain numbers, lowecase letters, and '/', Cant have more than one '/' together
                "/",
                "/apple/",
                "/",
                "/bull/",
                "/",
                "/banana/",
                "/fig/",
                "/lemon/lime/",
                "/",
                "/",
                "/blackberry/",
                "/blackberry/apricot/",
                "/blackberry/apricot/lime/",
                "/blackberry/apricot/lime/peach/",
                "/",
                "/tomato/",
                "/tomato/watermelon/",
                "/tomato/watermelon/cauliflower/",
                "/celery34/",
                "/",
                "/lettuce222/",
                "/",
                "/peas66/",
                "/beetroot/beetroot/",
                "/",
                "/.git/",
                "/no.one/",
                "/err.dev/no.one/",
                "/TheNewOrder/",
                "/Last.Order/.dev/",
                "/New.Order/dev/",
            };

        public static string[] validLocationsPrevPrev = new string[]
            {
                "/", // Cant be null or empty, Must start with '/', Must end with '/', Only can contain numbers, lowecase letters, and '/', Cant have more than one '/' together
                "/",
                "/",
                "/",
                "/",
                "/",
                "/",
                "/",
                "/lemon/",
                "/",
                "/",
                "/",
                "/blackberry/",
                "/blackberry/apricot/",
                "/blackberry/apricot/lime/",
                "/",
                "/",
                "/tomato/",
                "/tomato/watermelon/",
                "/",
                "/",
                "/",
                "/",
                "/",
                "/beetroot/",
                "/",
                "/",
                "/",
                "/err.dev/",
                "/",
                "/Last.Order/",
                "/New.Order/",
            };

        public static string[] validResourcesLocation = new string[]
            {
                "/", // Cant be null or empty, Must start with '/', Cant end with '/', Only can contain numbers, lowecase letters, and '/', Cant have more than one '/' together
                "/cats/",
                "/",
                "/bull/",
                "/",
                "/birds/",
                "/boar/",
                "/camel/cat/",
                "/",
                "/",
                "/beaver/",
                "/beaver/canary/",
                "/beaver/canary/chicken/",
                "/beaver/canary/chicken/cow/",
                "/",
                "/goose/",
                "/goose/gorilla/",
                "/goose/gorilla/goldfish/",
                "/cat34/",
                "/",
                "/bull222/",
                "/",
                "/bird66/",
                "/cats/",  // Cant have '.' in the last part, but no together, and cant end with it
                "/",
                "/bull/",
                "/",
                "/birds/",
                "/boars/",
                "/camel/cat/",
                "/",
                "/",
                "/.git/",
                "/no.one/",
                "/err.dev/no.one/",
                "/TheNewOrder/",
                "/Last.Order/.dev/",
                "/New.Order/dev/",
            };

        public static string[] validResourcesLocationPrev = new string[]
            {
                "/", // Cant be null or empty, Must start with '/', Cant end with '/', Only can contain numbers, lowecase letters, and '/', Cant have more than one '/' together
                "/",
                "/",
                "/",
                "/",
                "/",
                "/",
                "/camel/",
                "/",
                "/",
                "/",
                "/beaver/",
                "/beaver/canary/",
                "/beaver/canary/chicken/",
                "/",
                "/",
                "/goose/",
                "/goose/gorilla/",
                "/",
                "/",
                "/",
                "/",
                "/",
                "/",  // Cant have '.' in the last part, but no together, and cant end with it
                "/",
                "/",
                "/",
                "/",
                "/",
                "/camel/",
                "/",
                "/",
                "/",
                "/",
                "/err.dev/",
                "/",
                "/Last.Order/",
                "/New.Order/",
            };



        public static void DeleteLocations(string[] locations, DalilaFS fs)
        {
            var i = 0;
            var deleted = 0;

            while (deleted < locations.Length && i <50)
            {
                deleted = 0;
                i++;

                foreach (var location in locations)
                {
                    // Dont delete root
                    if (location == "/")
                    {
                        deleted++;
                        continue;
                    }

                    if (Directory.Exists(fs.LocationToSystemPath(location)))
                        Directory.Delete(fs.LocationToSystemPath(location), true);
                }

                locations = locations.Select(l => DalilaFS.GetPrevLocation(l)).ToArray();

            }

            
        }


        public static void CreateLocations(string[] locations, DalilaFS fs)
        {
            foreach (var location in locations)
            {
                Directory.CreateDirectory(fs.LocationToSystemPath(location));
            }
        }


        public static void DeleteResources(string[] resources, DalilaFS fs)
        {
            var i = 0;
            var deleted = 0;

            // Delete the files
            foreach(var resource in resources)
            {
                
                if (File.Exists(fs.ResourceToSystemPath(resource)))
                {
                    File.Delete(fs.ResourceToSystemPath(resource));
                }else
                {
                }
            }

            var locations = resources.Select(l => DalilaFS.GetResourceLocation(l)).ToArray();

            // Delete the directories
            while (deleted < locations.Length && i < 50)
            {
                deleted = 0;
                i++;

                foreach (var location in locations)
                {
                    // Dont delete root
                    if (location == "/")
                    {
                        deleted++;
                        continue;
                    }

                    if (Directory.Exists(fs.LocationToSystemPath(location)))
                        Directory.Delete(fs.LocationToSystemPath(location), true);
                }

                locations = locations.Select(l => DalilaFS.GetPrevLocation(l)).ToArray();

            }


        }


        public static void CreateResources(string[] resources, DalilaFS fs)
        {
            foreach (var resource in resources)
            {

                Directory.CreateDirectory(fs.LocationToSystemPath(DalilaFS.GetResourceLocation(resource)));

                using (FileStream fstream = File.Create(fs.ResourceToSystemPath(resource)))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("Random text.");
                    // Add some information to the file.
                    fstream.Write(info, 0, info.Length);
                }
            }
        }

    }

}
