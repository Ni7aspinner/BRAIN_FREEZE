import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

const Login = () => {
    const [username, setUsername] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        const token = localStorage.getItem("token");
        if (token) {
            navigate("/home");
        }
    }, [navigate]);

    const fetchNewSessionId = async () => {
        try {
            console.log('Requesting new session ID from session API');
            const response = await fetch("https://localhost:7005/api/session/new");
            
            if (!response.ok) {
                throw new Error(`Error fetching session ID: ${response.statusText}`);
            }

            const data = await response.json();
            if (data && data.sessionId) {
                localStorage.setItem("sessionId", data.sessionId);
                console.log("New session ID stored:", data.sessionId);
            } else {
                console.error("Session ID not found in response:", data);
            }
        } catch (error) {
            console.error("Error fetching new session ID:", error);
        }
    };

    const handleLogin = async () => {
        if (!username.trim()) {
            alert("Please enter a username.");
            return;
        }

        try {
            const response = await fetch("https://localhost:5219/api/user/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(username),
            });

            if (response.ok) {
                console.log(`Username: ${username}`);
                localStorage.setItem("token", username);
                
                await fetchNewSessionId();

                navigate("/home");
            } else {
                alert("Error: " + (await response.text()));
            }
        } catch (error) {
            console.error("Error logging in:", error);
        }
    };

    return (
        <div className="center">
            <div>
                <input
                    type="text"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    placeholder="Enter username"
                />
                <button onClick={handleLogin}>Login</button>
            </div>
        </div>
    );
};

export default Login;
