import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

const Login = () => {
    const [username, setUsername] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        const token = localStorage.getItem("ID");
        if (token) {
            navigate("/home");
        }
        else {
            console.log("User not found returning to login");
        }
    }, [navigate]);

    const fetchNewSessionId = async () => {
        try {
            const response = await fetch("https://localhost:7005/api/session/new");
            if (!response.ok) {
                throw new Error(`Error fetching session ID: ${response.statusText}`);
            }

            const data = await response.json();
            if (data.sessionId) {
                localStorage.setItem("sessionId", data.sessionId);
                console.log("New session ID stored:", data.sessionId);
            } else {
                console.error("Session ID not found in response.");
            }
        } catch (error) {
            console.error("Error fetching new session ID:", error);
        }
    };

    const initializeDatabaseEntry = async (): Promise<number | null> => {
        try {
            const response = await fetch(`https://localhost:7005/api/Scoreboards`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    place: 0,
                    username,
                    simonScore: 0,
                    cardflipScore: 999999,
                    nrgScore: 0,
                }),
            });
    
            if (!response.ok) {
                console.error("Error initializing database entry:", await response.text());
                return null;
            }
    
            const newUser = await response.json();
            console.log("New user added to the database:", newUser);
            return newUser.id;
        } catch (error) {
            console.error("Error initializing database entry:", error);
            return null;
        }
    };
    
    const fetchUserScores = async () => {
        try {
            const response = await fetch(`https://localhost:7005/api/Scoreboards/get-by-username/${username}`);
            
            if (!response.ok) {
                if (response.status === 404) {
                    console.log("Trying to create a new user");
                    const newUserId = await initializeDatabaseEntry();

                    const user = { id: newUserId, username };
                    if(user.id){
                        localStorage.setItem("ID", user.id.toString());
                        console.log(`Logged in as user ID: ${user.id}`);

                        await fetchNewSessionId();
                        navigate("/home");
                        return;
                    } else {
                        throw new Error(`Error fetching scores: ${response.statusText}`);
                    }
                }
            }
    
            let user = await response.json();
            
            if (!user || !user.id) {
                console.log(`User not found. Creating entry for ${username}.`);
                const newUserId = await initializeDatabaseEntry();
    
                if (newUserId === null) {
                    alert("Failed to create user. Please try again.");
                    return;
                }
    
                user = { id: newUserId, username };
            }
    
            localStorage.setItem("ID", user.id.toString());
            console.log(`Logged in as user ID: ${user.id}`);
    
            await fetchNewSessionId();
            navigate("/home");
    
        } catch (error) {
            console.error("Error fetching user scores:", error);
            console.log("Entering offline mode");
            localStorage.setItem("ID", "-1");
            console.log(`Logged in as user ID: -1`);
            navigate("/home");
        }
    };
    
    
    

    const handleLogin = async () => {
        if (!username.trim()) {
            alert("Please enter a username.");
            return;
        }

        await fetchUserScores();
    };

    const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === "Enter") {
            handleLogin();
        }
    };

    return (
        <div className="center">
            <div className="login-container">
                <div className="login-box">
                    <h2>Welcome to BRAINFREEZE</h2>
                    <p>Please enter your username to start playing.</p>
                    <input
                        type="text"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        placeholder="Enter username"
                        className="login-input"
                        onKeyDown={handleKeyDown}
                    />
                    <button onClick={handleLogin} className="login-button">
                        Login
                    </button>
                </div>
            </div>
        </div>
    );
    
};

export default Login;
