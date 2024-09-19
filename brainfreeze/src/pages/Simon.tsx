import React, { useState } from 'react';
import './Simon.css';
import Keypad from '../assets/Keypad.png';
import Follow from '../assets/Follow.png';

function Simon() {
  const [flashingButtons, setFlashingButtons] = useState(Array(9).fill(false));

  const buttonPositions = [
    { top: '40%', left: '28.5%' },
    { top: '40%', left: '51%' },
    { top: '40%', left: '71%' },
    { top: '58%', left: '28.5%' },
    { top: '58%', left: '51%' },
    { top: '58%', left: '71%' },
    { top: '77.5%', left: '28.5%' },
    { top: '77.5%', left: '51%' },
    { top: '77.5%', left: '71%' },
  ];

  const handleFlash = (index: number) => {
    setFlashingButtons((prev) => {
      const newState = [...prev];
      newState[index] = true;
      return newState;
    });

    setTimeout(() => {
      setFlashingButtons((prev) => {
        const newState = [...prev];
        newState[index] = false;
        return newState;
      });
    }, 200);
  };

  return (
    <>
      <div className="image-container">
        <img src={Follow} alt="Follow Image" className="image" />
        {buttonPositions.map((pos, index) => (
          <button
            key={index}
            className={`image-button ${flashingButtons[index] ? 'flashing' : ''}`}
            style={{ top: pos.top, left: pos.left, width: '50px', height: '50px' }}
            onClick={() => handleFlash(index)}
          >
          </button>
        ))}
      </div>
      <div className="image-container">
        <img src={Keypad} alt="Keypad Image" className="image" />
        {buttonPositions.map((pos, index) => (
          <button
            key={index}
            className="image-button"
            style={{ top: pos.top, left: pos.left, width: '50px', height: '50px' }}
            onClick={() => handleFlash(index)}
          >
          </button>
        ))}
      </div>
    </>
  );
}

export default Simon;
