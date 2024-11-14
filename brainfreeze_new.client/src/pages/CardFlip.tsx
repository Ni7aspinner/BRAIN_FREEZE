import './CardFlip.css';
import bf from '../assets/bf.png';
import gb from '../assets/gb.png';
import ic from '../assets/icecube.png';
import th from '../assets/think.png';
import sk from '../assets/skate.png';
import zz from '../assets/sleep.png';
import bb from '../assets/bigbrain.png';
import gr from '../assets/gear.png';
import cr from '../assets/icecream.png';
import { useState } from 'react';

const images = [
  bf, bf, gb,
  gb, ic, ic,
  th, th, sk,
  sk, zz, zz,
  bb, bb, gr,
  gr, cr, cr
];

const CardFlip: React.FC = () => {
  const [flippedCards, setFlippedCards] = useState<boolean[]>(
    new Array(images.length).fill(false)
  );

  const handleClick = (index: number) => {
    const newFlippedCards = [...flippedCards];
    newFlippedCards[index] = !newFlippedCards[index]; // Toggle the flip state
    setFlippedCards(newFlippedCards);
  };

  return (
    <div className="grid-container">
      {images.map((imageUrl, index) => (
        <div
          key={index}
          className={`card ${flippedCards[index] ? 'flipped' : ''}`}
          onClick={() => handleClick(index)}
        >
          <div className="card-inner">
            <div className="card-front">
              <div className="placeholder"></div>
            </div>
            <div className="card-back">
              <img src={imageUrl} alt={`Image ${index + 1}`} className="grid-image" />
            </div>
          </div>
        </div>
      ))}
    </div>
  );
};

export default CardFlip;
