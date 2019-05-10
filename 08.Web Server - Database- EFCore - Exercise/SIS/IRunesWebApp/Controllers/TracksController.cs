namespace IRunesWebApp.Controllers
{
    using IRunesWebApp.Models;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System;
    using System.Linq;
    using System.Web;

    public class TracksController : BaseController
    {
        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            var albumId = request.QueryData["albumId"].ToString();
            this.ViewBag["albumId"] = albumId;

            return this.View();
        }

        public IHttpResponse CreateTrackPostRequest(IHttpRequest request)
        {
            var trackName = HttpUtility.UrlDecode(request.FormData["name"].ToString().Trim());
            var link = HttpUtility.UrlDecode(request.FormData["link"].ToString());
            var price = Convert.ToDecimal(request.FormData["price"].ToString());

            //TODO Create TrackValidator

            var track = new Track()
            {
                Name = trackName,
                Link = link,
                Price = price
            };

            var albumId = request.QueryData["albumId"].ToString();
            var album = this.Db.Albums.FirstOrDefault(x => x.Id == albumId);

            if (album == null)
            {
                return new BadRequestResult("Album with provided Id does not exist", HttpResponseStatusCode.BadRequest);
            }

            var albumTrack = new AlbumTrack()
            {
                Track = track,
                Album = album
            };

            this.Db.AlbumTracks.Add(albumTrack);

            album.Price = album.Tracks.Sum(x => x.Track.Price) * 0.80m; 
         
            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return new BadRequestResult(e.Message, HttpResponseStatusCode.BadRequest);
            }

            return new RedirectResult($"/albums/details?id={album.Id}");
        }
    }
}