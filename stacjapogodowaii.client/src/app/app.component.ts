import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  summary: string;
  humidity: number;
  windSpeed: number;
  location: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public forecasts: { [location: string]: WeatherForecast[] } = {};
  public locations: string[] = ["NewYork", "LosAngeles", "Chicago"]; // Add your list of locations here
  public selectedLocation: string = "NewYork"; // Default selected location

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.locations.forEach(location => {
      this.getForecasts(location);
    });
  }

  getForecasts(location: string) {
    this.http.get<WeatherForecast[]>(`http://localhost:7267/WeatherForecast/7Days/${location}`).subscribe(
      (result) => {
        this.forecasts[location] = result;
      },
      (error) => {
        console.error(`Error fetching forecasts for ${location}:`, error);
      }
    );
  }

  // Method triggered when location selection changes
  onLocationChange(selectedLocation: string) {
    this.selectedLocation = selectedLocation; // Update the selected location
    if (!this.forecasts[selectedLocation]) {
      this.getForecasts(selectedLocation); // Call getForecasts if forecasts for the selected location are not available
    }
  }
}
