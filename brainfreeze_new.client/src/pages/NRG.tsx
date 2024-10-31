import { useState} from 'react';
import './NRG.css';
import Grid from '../assets/Grid-1000-10-2-100.png';

function NRG() {
    const [flashingButtons, setFlashingButtons] = useState(Array(25).fill(false));
    const buttonPositions = [
        { top: '0.2%', left: '0%' },
        { top: '0.2%', left: '20%' },
        { top: '0.2%', left: '40%' },
        { top: '0.2%', left: '60%' },
        { top: '0.2%', left: '80%' },

        { top: '20%', left: '0%' },
        { top: '20%', left: '20%' },
        { top: '20%', left: '40%' },
        { top: '20%', left: '60%' },
        { top: '20%', left: '80%' },

        { top: '39.8%', left: '0%' },
        { top: '39.8%', left: '20%' },
        { top: '39.8%', left: '40%' },
        { top: '39.8%', left: '60%' },
        { top: '39.8%', left: '80%' },
        
        { top: '59.6%', left: '0%' },
        { top: '59.6%', left: '20%' },
        { top: '59.6%', left: '40%' },
        { top: '59.6%', left: '60%' },
        { top: '59.6%', left: '80%' },
        
        { top: '79.3%', left: '0%' },
        { top: '79.3%', left: '20%' },
        { top: '79.3%', left: '40%' },
        { top: '79.3%', left: '60%' },
        { top: '79.3%', left: '80%' },
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
      <h3>Memory Matrix</h3>
      <div className="block" >
            <div className='image-container1'>
                
            <img src={Grid} className="imageGrid"/>
            {buttonPositions.map((pos, index) => (
          <button
                key={index}
                className={`grid-block ${flashingButtons[index] ? 'activated' : ''}`}
                style={{ top: pos.top, left: pos.left, width: '100px', height: '100px' }}
                onClick={() => {
                    handleFlash(index);
                  }}
          >
          </button>
        ))}
        </div>
        </div>
      </>
    );
  }
export default NRG;