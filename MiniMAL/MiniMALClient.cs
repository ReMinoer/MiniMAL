using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using MiniMAL.Exceptions;

namespace MiniMAL
{
    public class MiniMALClient
    {
        // http://myanimelist.net/malappinfo.php?status=all&type=anime&u=Luxiamimi
        // http://myanimelist.net/malappinfo.php?status=all&type=manga&u=Aeden

        private string _username;
        private string _password;

        public bool IsConnected { get; private set; }

        public MiniMALClient()
        {
            IsConnected = false;
        }

        public void Authentification(string username, string password)
        {
            string link = "http://myanimelist.net/api/account/verify_credentials.xml";
            WebRequest request = WebRequest.Create(link);
            request.Credentials = new NetworkCredential(_username, _password);

            try
            {
                WebResponse response = request.GetResponse();
            }
            catch (WebException e)
            {
                IsConnected = false;

                if (e.Message.Contains("401"))
                    throw new UserUnauthorizedException();
                else
                    throw e;
            }

            _username = username;
            _password = password;
            IsConnected = true;
        }

        public AnimeList LoadAnimelist(string user)
        {
            AnimeList list = new AnimeList();

            string link = "http://myanimelist.net/malappinfo.php?u=" + user + "&type=anime&status=all";
            XmlDocument xml = LoadXml(link);

            foreach (XmlNode e in xml.DocumentElement.ChildNodes)
            {
                if (e.Name == "anime")
                {
                    Anime a = new Anime();
                    a.LoadFromXmlNode(e);
                    list.Add(a);
                }
            }
            return list;
        }

        public MangaList LoadMangalist(string user)
        {
            MangaList list = new MangaList();

            string link = "http://myanimelist.net/malappinfo.php?u=" + user + "&type=manga&status=all";
            XmlDocument xml = LoadXml(link);

            foreach (XmlNode e in xml.DocumentElement.ChildNodes)
            {
                if (e.Name == "manga")
                {
                    Manga m = new Manga();
                    m.LoadFromXmlNode(e);
                    list.Add(m);
                }
            }
            return list;
        }

        public List<AnimeSearchEntry> SearchAnime(string[] search)
        {
            string link = "http://myanimelist.net/api/anime/search.xml?q=";

            if (search.Any())
                link += search[0];

            for (int i = 1; i < search.Length; i++)
                link += "+" + search[i];

            XmlDocument xml = LoadXmlWithCredentials(link);

            List<AnimeSearchEntry> list = new List<AnimeSearchEntry>();
            foreach (XmlNode e in xml.DocumentElement.ChildNodes)
            {
                if (e.Name == "entry")
                {
                    AnimeSearchEntry a = new AnimeSearchEntry();
                    a.LoadFromXmlNode(e);
                    list.Add(a);
                }
            }

            return list;
        }

        public List<MangaSearchEntry> SearchManga(string[] search)
        {
            string link = "http://myanimelist.net/api/manga/search.xml?q=";

            if (search.Any())
                link += search[0];

            for (int i = 1; i < search.Length; i++)
                link += "+" + search[i];

            XmlDocument xml = LoadXmlWithCredentials(link);

            List<MangaSearchEntry> list = new List<MangaSearchEntry>();
            foreach (XmlNode e in xml.DocumentElement.ChildNodes)
            {
                if (e.Name == "entry")
                {
                    MangaSearchEntry m = new MangaSearchEntry();
                    m.LoadFromXmlNode(e);
                    list.Add(m);
                }
            }

            return list;
        }

        private XmlDocument LoadXml(string link)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(link);
            return xml;
        }

        private XmlDocument LoadXmlWithCredentials(string link)
        {
            if (!IsConnected)
                throw new UserNotConnectedException();

            WebRequest request = WebRequest.Create(link);
            request.Credentials = new NetworkCredential(_username, _password);

            XmlDocument xml = new XmlDocument();
            StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream());
            string content = sr.ReadToEnd().Replace("&mdash;", "&#8212;").Replace("&forall;", "&#8704;");
            xml.LoadXml(content);

            return xml;
        }
    }
}