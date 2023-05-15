using WebAPITest.Data;
using WebAPITest.Models;
using WebAPITest.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


namespace WebAPITest.DataBase
{
    public class CardDB : ICardService
    {
        private readonly DataContext _context;
        private readonly ISaveChangesService _saveChanges;
        public CardDB(DataContext context, ISaveChangesService saveChanges)
        {
            _context = context;
            _saveChanges = saveChanges;
        }

        public async Task<ActionResult<IEnumerable<Card>>> GetAllCards()
        {
            return await _context.Cards.ToListAsync();
        }
        public async Task<ActionResult<Card>> GetOneCard(int cardId)
        {
            return await _context.Cards.FindAsync(cardId) is Card card
                ? card
                : new Card();
        }
        public async Task<ActionResult<IEnumerable<Card>>> GetCardByClient(string client)
        {
            return await _context.Cards.Where(card => card.Client == client).ToListAsync();
        }
        public async Task<bool> CardExist(int cardId)
        {
            var cards = await _context.Cards.FindAsync(cardId);
            return cards is not null;
        }
        public void AddCard(Card card)
        {
            _context.Add(card);
            _saveChanges.SaveChangesDatabase();
        }

        public void UpdateCard(Card card)
        {
            _context.Update(card);
            _saveChanges.SaveChangesDatabase();
        }

        public void DeleteCard(Card card)
        {
            _context.Remove(card);
            _saveChanges.SaveChangesDatabase();
        }
    }
}