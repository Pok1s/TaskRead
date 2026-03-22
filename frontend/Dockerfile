# Dev: Vite — przeglądarka na hoście woła API na localhost:5050 (mapowanie portu z kontenera api).
FROM node:20-alpine
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm ci
COPY . .
EXPOSE 5173
ENV VITE_API_URL=http://localhost:5050
CMD ["npm", "run", "dev", "--", "--host", "0.0.0.0", "--port", "5173"]
