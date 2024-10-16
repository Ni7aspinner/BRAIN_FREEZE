import './Game.css'
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

const Game: React.FC = () => {
  const [visibleImages, setVisibleImages] = useState<boolean[]>(
    new Array(images.length).fill(false)
  );

  const handleClick = (index: number) => {
    const newVisibleImages = [...visibleImages];
    newVisibleImages[index] = true; 
    setVisibleImages(newVisibleImages);
  };

  return (
    <div className="grid-container">
      {images.map((imageUrl, index) => (
        <div key={index} className="grid-item" onClick={() => handleClick(index)}>
          {visibleImages[index] ? (
            <img src={imageUrl} alt={`Image ${index + 1}`} className="grid-image" />
          ) : (
            <div className="placeholder"></div>
          )}
        </div>
      ))}
    </div>
  );
};


export default Game
