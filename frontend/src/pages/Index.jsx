import React, { useEffect, useState } from "react";
import ReactMarkdown from "react-markdown";
import remarkGfm from "remark-gfm";
import rehypeRaw from "rehype-raw"; // 👈 allow raw HTML inside markdown

export default function IndexPage() {
    const [content, setContent] = useState("");

    useEffect(() => {
        fetch("/README.md")
            .then((res) => res.text())
            .then((text) => setContent(text));
    }, []);

    return (
        <div className="w-full mt-6">
            <article className="prose prose-lg dark:prose-invert text-left">
                <ReactMarkdown
                    remarkPlugins={[remarkGfm]}
                    rehypePlugins={[rehypeRaw]}
                >
                    {content}
                </ReactMarkdown>
            </article>
        </div>
    );
}
