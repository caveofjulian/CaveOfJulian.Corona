# CaveOfJulian.Corona

CaveOfJulian.Corona is split into three  parts.
The Corona data is gathered through web scraping in Corona.DataIngestion and analyzed with machine learning in the Corona.MachineLearning project.

The endpoints are exposed through an ASP Web Api in Corona.Api. 

The data ingestion project is responsible for scraping the new data about corona from [a Github repository.](https://github.com/CSSEGISandData/COVID-19/)

The Web Api is able to create reports on demand about this data.

## Possible Improvements
I'm very aware that the data repository returns IQueryable which makes the rest of the project tightly coupled to Entity Framework. 
I decided to keep it like this as it's not an issue at all currently. Feel free to fix this in a pull request. 

## Installation

Clone this repository and run it on the right IP and port :) 


## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## Contributers
CaveOfJulian - Project Leader and currently the only contributer
## License
[GNU General Public License v3.0](https://choosealicense.com/licenses/gpl-3.0/)
