namespace IRunesWebApp.Controllers
{
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System.Linq;
    using System;
    using IRunesWebApp.Common.Validators;
    using SIS.HTTP.Enums;
    using IRunesWebApp.Models;
    using System.Web; 

    public class AlbumsController : BaseController
    {
        public IHttpResponse All(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            var albums = this.Db.Albums.ToList();

            var content = "<ol>";
            foreach (var album in albums)
            {
                content += $"<li><a href=\"/albums/details?id={album.Id}\">" + album.Name + "</a></li>";
            }
            content += "</ol>";

            if (!albums.Any())
            {
                content = "<p>Nothing to show...</p>" +
                        Environment.NewLine +
                        "<p>Your albums section is empty!</p>";
            }

            this.ViewBag["albums"] = content;

            return this.View();
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            return this.View();
        }

        public IHttpResponse CreateAlbumPostRequest(IHttpRequest request)
        {
            var albumName = HttpUtility.UrlDecode(request.FormData["name"].ToString().Trim());
            var coverUrl = HttpUtility.UrlDecode(request.FormData["cover"].ToString());

            if (!AlbumValidator.IsAlbumValid(albumName))
            {
                return new BadRequestResult("Album name can not be empty!", HttpResponseStatusCode.BadRequest);
            }
            else if (!AlbumValidator.IsCoverUrlValid(coverUrl))
            {
                return new BadRequestResult("Provided cover url is not valid!", HttpResponseStatusCode.BadRequest);
            }

            var album = new Album()
            {
                Name = albumName,
                Cover = coverUrl
            };

            this.Db.Albums.Add(album);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return new BadRequestResult(e.Message, HttpResponseStatusCode.BadRequest);
            }

            return new RedirectResult("/albums/all");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }
            
            var id = request.QueryData["id"].ToString();

            var album = this.Db.Albums.FirstOrDefault(x => x.Id == id);
            if (album == null)
            {
                return new BadRequestResult("Album with given Id does not exist!", HttpResponseStatusCode.BadRequest);
            }

            this.ViewBag["albumCoverUrl"] = album.Cover;
            this.ViewBag["name"] = album.Name;
            this.ViewBag["price"] = album.Price.ToString();
            this.ViewBag["albumId"] = album.Id; 

            var tracks = album.Tracks.ToList();

            var tracksContent = "<ul>";
            var counter = 1; 
            foreach (var track in tracks)
            {
                tracksContent += $"<li>{counter++}. <a href=\"/tracks/details?id={track.TrackId}\">" + track.Track.Name + "</a></li>";
            }
            tracksContent += "</ul>";

            if (!tracks.Any())
            {
                tracksContent = "<p>Nothing to show...</p>" +
                        Environment.NewLine +
                        "<p>This album has no tracks added yet!</p>";
            }

            this.ViewBag["tracks"] = tracksContent;

            return this.View();
        }
    }
}