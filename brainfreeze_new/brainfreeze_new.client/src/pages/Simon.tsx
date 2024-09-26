import { useState, useEffect } from 'react';
import './Simon.css';
import Keypad from '../assets/keypad.png';
import Follow from '../assets/follow.png';
interface Data {
    number: number; 
}
function Simon() {
  const [datas, setData] = useState<Data[] | undefined>(undefined);
  const [dataString, setDataString] = useState<string>(''); 

  useEffect(() => {
      populateData();
  }, []);

  const contents = datas === undefined
      ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
      : <div>{dataString}</div>; 


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

    const handleArray = () => {
      if (datas && datas.length >= 12) {
      for(let i=0; i<datas.length; i++)
      {
        setTimeout(() => { handleFlash(datas[i].number-1);}, i*400);
      }
      setTimeout(() => {
        fetch('https://localhost:5276/api/buttonpress');
    }, 12 * 400);
    } else{
      console.error("Datas is either undefined or doesn't have enough elements");
    }
  };
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

        fetch('https://localhost:5276/api/buttonpress')
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
          <div>
              {contents}
              <button onClick={() => handleArray()}></button>
          </div>
    </>
  );
  async function populateData() {
    try {
        const response = await fetch('/Inc');
        const data: Data[] = await response.json(); 
        setData(data); 
        const dataString = data.map(item => item.number).join(', '); 
        setDataString(dataString);
    } catch (error) {
        console.error("Failed to fetch data", error);
    }
  }
}

export default Simon;
