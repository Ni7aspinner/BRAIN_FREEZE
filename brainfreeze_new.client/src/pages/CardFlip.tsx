import { useEffect, useState } from 'react';
import './CardFlip.css';

const CardFlip = () => {
  const [images, setImages] = useState<string[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [flippedCards, setFlippedCards] = useState<boolean[]>([]);
  const [selectedCards, setSelectedCards] = useState<number[]>([]);
  const [matchedCards, setMatchedCards] = useState<boolean[]>([]);
  const [moveCount, setMoveCount] = useState<number>(0);
  const [highScore, setHighScore] = useState<number | null>(null);
  const [isResetting, setIsResetting] = useState<boolean>(false);
  const [isReady, setIsReady] = useState<boolean>(true); // Track when the game is ready to display cards

  const fetchShuffledImages = async () => {
    try {
      const response = await fetch('https://localhost:5219/api/cardflip/shuffledImages');
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      const data = await response.json();

      setTimeout(() => {
        setImages(data.shuffledImages);
        setFlippedCards(new Array(data.shuffledImages.length).fill(false));  // Reset flipped state
        setMatchedCards(new Array(data.shuffledImages.length).fill(false));  // Reset matched state
        setIsResetting(false);
        setIsReady(true); 
      }, 500);
    } catch (err: any) {
      setError(err.message);
    }
  };

  
  useEffect(() => {
    fetchShuffledImages();

    const fetchHighScore = async () => {
      try {
        const highScoreResponse = await fetch('https://localhost:5219/api/cardflip/highscore');
        if (highScoreResponse.ok) {
          const highScoreData = await highScoreResponse.json();
          setHighScore(highScoreData.highScore);
        }
      } catch (err) {
        setError('Error fetching high score');
      }
    };

    fetchHighScore();
  }, []);

  
  const handleCardClick = (index: number) => {
    if (isResetting || selectedCards.length === 2 || matchedCards[index]) return;

    const newFlippedCards = [...flippedCards];
    newFlippedCards[index] = true;
    setFlippedCards(newFlippedCards);

    const newSelectedCards = [...selectedCards, index];
    setSelectedCards(newSelectedCards);

    if (newSelectedCards.length === 2) {
      setMoveCount(prevCount => prevCount + 1);
      checkForMatch(newSelectedCards);
    }
  };

  
  const checkForMatch = (selected: number[]) => {
    const [firstIndex, secondIndex] = selected;
    if (images[firstIndex] === images[secondIndex]) {
      const newMatchedCards = [...matchedCards];
      newMatchedCards[firstIndex] = true;
      newMatchedCards[secondIndex] = true;
      setMatchedCards(newMatchedCards);
      setSelectedCards([]);

      if (newMatchedCards.every(Boolean)) {
        submitScore(moveCount + 1);  
      }
    } else {
      setTimeout(() => {
        const newFlippedCards = [...flippedCards];
        newFlippedCards[firstIndex] = false;
        newFlippedCards[secondIndex] = false;
        setFlippedCards(newFlippedCards);
        setSelectedCards([]);
      }, 1000);
    }
  };


  const submitScore = async (finalScore: number) => {
    try {
      const response = await fetch('https://localhost:5219/api/cardflip/submitScore', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ score: finalScore })
      });

      if (response.ok) {
        const data = await response.json();
        if (data.newHighScore) {
          setHighScore(finalScore); 
        }
      }
    } catch (err) {
      setError('Failed to submit score');
    }
  };

  const resetGame = () => {
    setIsReady(false);  
    setIsResetting(true); 
    setMoveCount(0);
    setSelectedCards([]);
    setFlippedCards(new Array(images.length).fill(true));  
    setTimeout(fetchShuffledImages, 500);  
  };

  return (
    <div>
      <h2>Card Flip Game</h2>

      {error && <div style={{ color: 'red' }}>{error}</div>}

      <div className="score-board">
        <p>Moves: {moveCount}</p>
        {highScore !== null && <p>High Score: {highScore}</p>}
        <button onClick={resetGame}>Restart Game</button>
      </div>

      <div className="grid-container">
        {images.length > 0 && isReady ? (
          images.map((imageUrl, index) => (
            <div
              key={index}
              className={`card ${flippedCards[index] || matchedCards[index] ? 'flipped' : ''}`}
              onClick={() => handleCardClick(index)}
            >
              <div className="card-inner">
                <div className="card-front">
                  <div className="placeholder"></div>
                </div>
                <div className="card-back">
                  <img src={`/${imageUrl}`} alt={`Image ${index + 1}`} className="grid-image" />
                </div>
              </div>
            </div>
          ))
        ) : (
          <p>Loading images...</p>
        )}
      </div>
    </div>
  );
};

export default CardFlip;
