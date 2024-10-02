import { useState, useEffect } from 'react';
import './Simon.css';
import Keypad from '../assets/keypad.png';
import Follow from '../assets/follow.png';

interface Data {
    createdList: number[];
    level: number;
    expectedList: number[]; 
}

function Simon() {

  const [datas, setData] = useState<Data>();
  const [dataString1, setDataString1] = useState<string>(''); 
  const [dataString2, setDataString2] = useState<string>(''); 
  const [flashingButtons, setFlashingButtons] = useState(Array(9).fill(false));
  const contents = datas === undefined
      ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
      : (<div><div></div>Game id: {localStorage.getItem('sessionId')}<div>{dataString1}</div><div>{dataString2}</div></div> ); 
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

  // Currently populates twice because main.tsx has strict mode enabled, useful for debugging (idk)
  // so dont be scared of the browser console output
  useEffect(() => {
    console.log('Checking for session ID in local storage');
    const storedSessionId = localStorage.getItem('sessionId');
    if (!storedSessionId) {
      console.log('No session ID found in local storage... Fetching new session ID', storedSessionId);
      fetchNewSessionId();
    } 
    else {
      console.log('Session ID found in local storage:', storedSessionId);
      populateData(storedSessionId);
    }
  }, []);

  // Talks to the session api to get a new session id if it is not stored in the browser
  const fetchNewSessionId = async () => {
    try {
        console.log('Requesting new session ID from session API');
        const response = await fetch(`https://localhost:7005/api/session/new`);
        
        if (!response.ok) {
            throw new Error(`Error fetching session ID: ${response.statusText}`);
        }

        const data = await response.json();
        if (data && data.sessionId) {
            localStorage.setItem('sessionId', data.sessionId);
            console.log('New session ID stored:', data.sessionId);
            populateData(data.sessionId);
        } else {
            console.error('Session ID not found in response:', data);
        }
    } catch (error) {
        console.error('Error fetching new session ID:', error);
    }
  };

  // Takes sessionId and identifies which session to use to fetch data and then process it
  const populateData = async (sessionId: string) => {
    try {
        console.log('Populating data');
        const response = await fetch(`https://localhost:7005/api/Inc?sessionId=${sessionId}`);  // Bijau liest sita, jis kolkas veikia,
        if (!response.ok) {                                                                     // gal veliau prireiks darant logika
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        const data: Data = await response.json();
        setData(data);
        const dataString1 = data.createdList.join(', ');
        setDataString1(dataString1);
    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
  };

  // Handles the flashing based on the array
  const handleArray = () => {
    if (datas && datas.level >=4) {
      for(let i=0; i<datas.level; i++){
        setTimeout(() => { handleFlash(datas.createdList[i]-1);}, i*400);
      }
      setTimeout(() => {
        //fetch('https://localhost:7005/api/buttonpress');
      }, 12 * 400);
    } 
    else{
      console.error("Data is either undefined or doesn't have enough elements");
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
    //fetch('https://localhost:5276/api/buttonpress')
  };


  // Sends data to an API :Shrugs:
  async function postData(data : Data) 
  {
    try {
      console.log('Posting data:', JSON.stringify(data));
      const response = await fetch('https://localhost:5219/api/Inc', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(data),
      });

      if (response.ok) {
      const result = await response.json();
      console.log('API response: ', result);
      setData(result);
      const dataString1 = result.createdList.join(', '); 
      setDataString1(dataString1); 
      const dataString2 = result.expectedList.join(', ');
      setDataString2(dataString2);
      }
      else {
        console.error('Error in API request: ', response.statusText);
      }
    } 
    catch (error) {
      console.error("Failed to fetch data", error);
    }
  }

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
                onClick={() => {
                  handleFlash(index);
                  if (datas) {
                    const updatedData = {
                      ...datas,
                      expectedList: Array.isArray(datas.expectedList) ? [...datas.expectedList, index + 1] : [index + 1],  
                    };
                    setData(updatedData);
                    const dataString2 = updatedData.expectedList.join(', ');
                    setDataString2(dataString2);
                    if (updatedData.expectedList.length == datas.level) {
                      postData(updatedData);  
                      const updatedData1 = {
                        ...datas,
                        expectedList: [],  
                      };
                      setData(updatedData1);
                    }
                  }
                }}
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
}

export default Simon;
