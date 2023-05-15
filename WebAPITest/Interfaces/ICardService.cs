using Microsoft.AspNetCore.Mvc;
using WebAPITest.Models;

namespace WebAPITest.Interfaces
{
    public interface ICardService
    {
        Task<ActionResult<IEnumerable<Card>>> GetAllCards();
        Task<ActionResult<Card>> GetOneCard(int cardId);
        Task<ActionResult<IEnumerable<Card>>> GetCardByClient(string client);
        Task<bool> CardExist(int cardId);
        void AddCard(Card card);
        void UpdateCard(Card card);
        void DeleteCard(Card card);
    }
}