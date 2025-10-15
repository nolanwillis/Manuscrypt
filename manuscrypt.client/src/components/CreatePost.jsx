import React, { useState, useRef } from 'react';

export default function CreatePost({ onPostCreated }) {
    const titleRef = useRef('');
    const descriptionRef = useRef('');
    const [file, setFile] = useState(null);
    const [error, setError] = useState(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleFileChange = (e) => {
        setFile(e.target.files[0] || null);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsSubmitting(true);
        setError(null);
        try {
            const userId = sessionStorage.getItem("userId");
            if (!userId) {
                throw new Error('No userId found.');
            }
            if (!file) {
                throw new Error('File is required.');
            }
            const title = titleRef.current.value;
            if (!title) {
                throw new Error('Title is required.');
            }
            const description = descriptionRef.current.value;
            let publishedAt = new Date().toISOString();
            let fileName = file.name;
            let fileType = file.type;
            let fileSizeBytes = file.size;

            const response = await fetch('http://localhost:5125/post', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    userId,
                    title,
                    description,
                    publishedAt,
                    fileUrl: 'empty.com', // Blank for now
                    fileName,
                    fileType,
                    fileSizeBytes,
                }),
            });

            if (!response.ok) {
                const errorMsg = await response.text();
                throw new Error(errorMsg || 'Failed to create post');
            }


            titleRef.current.value = '';
            descriptionRef.current.value = '';
            setFile(null);

            if (onPostCreated) {
                onPostCreated(createdPost);
            }
        } catch (err) {
            setError(err.message);
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <form
            onSubmit={handleSubmit}
            className="bg-white rounded-2xl shadow-lg p-8 w-full max-w-md flex flex-col space-y-6"
            style={{ minWidth: 320 }}
        >
            <h2 className="text-2xl font-bold mb-2 text-center text-black">Create Post</h2>
            {error && <div className="text-red-600 text-center">{error}</div>}
            <div>
                <label htmlFor="post-title" className="block mb-1 font-medium text-black">Title</label>
                <input
                    id="post-title"
                    type="text"
                    ref={titleRef}
                    disabled={isSubmitting}
                    className="w-full px-4 py-2 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-400 text-black"
                    placeholder="Enter post title"
                />
            </div>
            <div>
                <label htmlFor="post-description" className="block mb-1 font-medium text-black">Description</label>
                <textarea
                    id="post-description"
                    ref={descriptionRef}
                    disabled={isSubmitting}
                    className="w-full px-4 py-2 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-400 resize-none text-black"
                    rows={4}
                    placeholder="Enter post description"
                />
            </div>
            <div>
                <label htmlFor="post-file" className="block mb-1 font-medium text-black">Upload File</label>
                <input
                    id="post-file"
                    type="file"
                    onChange={handleFileChange}
                    disabled={isSubmitting}
                    className="w-full rounded-lg border border-gray-300 file:rounded-lg file:border-none file:bg-blue-600 file:text-white file:px-4 file:py-2 file:cursor-pointer text-black"
                />
            </div>
            <button
                type="submit"
                disabled={isSubmitting}
                className="w-full bg-blue-600 text-white font-semibold py-2 rounded-lg hover:bg-blue-700 transition"
            >
                {isSubmitting ? 'Posting...' : 'Create Post'}
            </button>
        </form>
    );
}
