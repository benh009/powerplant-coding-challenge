# Powerplant-coding-challenge

## Dynamic Programming Method
I implemented a  Dynamic Programming Method algorithm to find a combination. 

Here we can find the documentation that I used : https://www.eeeguide.com/optimal-unit-commitment-uc/

Here is the summary of the algorithm : 

Let a cost function FN (x) be defined as follows:

FN (x) = the minimum cost in Rs/hr of generating x MW by N units

fN (y) = cost of generating y MW by the Nth unit

FN-1 (x – y) = the minimum cost of generating (x – y) MW by the remaining (N – 1) units

Now the application of DP results in the following recursive relation

![alt text](https://www.eeeguide.com/wp-content/uploads/2016/12/Optimal-Unit-Commitment-001.jpg)


## How to build & run the API

### Prerequis 

 .Net 7.0 
 
 docker 

### Build & Run dotnet
```
cd ./PowerPlantAPI
dotnet build
dotnet run
```

Swagger with documentation

http://localhost:8888/swagger/index.html

REST API endpoint /productionplan

http://localhost:8888/ProductionPlan

Curl :

```
curl -X 'POST' \
  'http://localhost:8888/ProductionPlan' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "load": 480,
  "fuels": {
    "gas(euro/MWh)": 13.4,
    "kerosine(euro/MWh)": 50.8,
    "co2(euro/ton)": 20,
    "wind(%)": 60
  },
  "powerplants": [
    {
      "Name": "gasfiredbig1",
      "Type": "gasFired",
      "Efficiency": 0.53,
      "Pmin": 100,
      "Pmax": 460
    },
    {
      "Name": "gasfiredbig2",
      "Type": "gasFired",
      "Efficiency": 0.53,
      "Pmin": 100,
      "Pmax": 460
    },
    {
      "Name": "gasfiredsomewhatsmaller",
      "Type": "gasFired",
      "Efficiency": 0.37,
      "Pmin": 40,
      "Pmax": 210
    },
    {
      "Name": "tj1",
      "Type": "turbojet",
      "Efficiency": 0.3,
      "Pmin": 0,
      "Pmax": 16
    },
    {
      "Name": "windpark1",
      "Type": "windTurbine",
      "Efficiency": 1,
      "Pmin": 0,
      "Pmax": 150
    },
    {
      "Name": "windpark2",
      "Type": "windTurbine",
      "Efficiency": 1,
      "Pmin": 0,
      "Pmax": 36
    }
  ]
}'
```

### Build & Run Docker
```
cd ./PowerPlantAPI
docker build -t powerplantapi . 
docker run -e "ASPNETCORE_ENVIRONMENT=Development" -p 8887:443 -p 8888:80 powerplantapi
```

 warning : 
 Add this following param in Docker Engine to build an image. 
 ```
  "dns": [
    "1.1.1.1",
    "8.8.8.8"
  ],
  ```