import React, { useState } from 'react';

export default function NavBar({ isLoggedIn, onLoginPress, onLogoutPress }) {
    
    return (
        <nav className="fixed top-0 left-0 bottom-0 w-64 z-50 bg-background/80 border-r flex flex-col">
            <div className="p-6 border-b">
                <span className="font-manuscript text-2xl font-bold text-foreground">
                    Manuscrypt
                </span>
            </div>

            <div className="flex-1 flex flex-col justify-between py-6">
                {isLoggedIn && (
                    <div className="space-y-2 px-4">
                        <a href="/" className="block px-4 py-3 rounded-lg">
                            Home
                        </a>
                        <a href="#subscriptions" className="block px-4 py-3 rounded-lg">
                            Subscriptions
                        </a>
                        <a href="#channels" className="block px-4 py-3 rounded-lg">
                            My Channel
                        </a>
                    </div>
                )}
                <div className="space-y-2 px-4">
                    <button className="w-full px-4 py-2 rounded-lg">
                        Settings
                    </button>
                    {isLoggedIn ? (
                        <button
                            className="w-full px-4 py-2 rounded-lg"
                            onClick={onLogoutPress}
                        >
                            Sign Out
                        </button>
                    ) : (
                        <button
                            className="w-full px-4 py-2 rounded-lg"
                            onClick={onLoginPress}
                        >
                            Sign In
                        </button>
                    )}
                </div>
            </div>
        </nav>
    );
}