import type { ReactNode } from "react";

type CardProps = {
  title: string;
  children: ReactNode;
  topContent?: ReactNode;
};

export function Card({ title, children, topContent }: CardProps) {
  return (
    <section className="card">
      {topContent ? <div className="card-top-content">{topContent}</div> : null}
      <h2 className="card-title">{title}</h2>
      <div className="card-content">{children}</div>
    </section>
  );
}