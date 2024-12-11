import { Link } from 'react-router-dom';
import './Navigationbar.css';
import { useState, useEffect } from 'react';
import unmutedIcon from '../assets/unmuted.png';
import mutedIcon from '../assets/muted.png';

function Header() {
    const [isMuted, setIsMuted] = useState(false);

    useEffect(() => {
        fetch('https://localhost:5219/api/Mute') 
            .then(response => response.json())
            .then(data => {
                setIsMuted(data.isMuted);  
            })
            .catch(error => {
                console.error('Error fetching mute state:', error);
            });
    }, []);

    const handleMuteToggle = () => {
        const newMuteState = !isMuted;

        fetch('https://localhost:5219/api/Mute', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ isMuted: newMuteState }),
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log('Mute state updated:', data);
        })
        .catch(error => {
            console.error('Error updating mute state:', error);
        });

        setIsMuted(newMuteState);
    };

    return (
        <header>
            <nav>
                <Link to="/home" className="home-arrow"></Link>
                <button className="mute-button" onClick={handleMuteToggle}>
                    <img 
                        src={isMuted ? unmutedIcon : mutedIcon} 
                        alt={isMuted ? 'Mute' : 'Unmute'} 
                    />
                </button>
            </nav>
        </header>
    );
}

export default Header;
