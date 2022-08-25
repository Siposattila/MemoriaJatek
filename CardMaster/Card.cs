namespace MemoriaJatek.CardMaster
{
    class Card
    {
        public string CardValue { get; private set; }
        public int CardNumber { get; set; }
        public bool IsHidden { get; set; }

        public Card(string cardValue, int cardNumber)
        {
            CardValue = cardValue;
            CardNumber = cardNumber;
            IsHidden = false;
        }

        public override string ToString()
        {
            return CardValue;
        }
    }
}
    